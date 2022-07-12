using AEAssist.Define;
using AEAssist.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI
{
    public interface IAISpellQueue
    {
        List<IAISpellQueueSlot> SlotQueue { get; }
    }

    public interface IAISpellQueueSlot
    {
        int Check(int index);
        void Fill(SpellQueueSlot slot);
    }


    public class AISpellQueueMgr
    {
        public static AISpellQueueMgr Instance = new AISpellQueueMgr();

        private Dictionary<Type, IAISpellQueue> AllQueues
            = new Dictionary<Type, IAISpellQueue>();



        public void Init()
        {
            AllQueues.Clear();
            var baseType = typeof(IAISpellQueue);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;

                AllQueues[type] = Activator.CreateInstance(type) as IAISpellQueue;
                LogHelper.Debug($"Load IAISpellQueue: {type.Name}");
            }
        }


        public void ClearApply()
        {
            AIRoot.GetBattleData<BattleData>().CurrApply = null;
            AIRoot.GetBattleData<BattleData>().ApplyIndex = 0;
        }

        public void Apply<T>() where T : IAISpellQueue
        {
            var type = typeof(T);
            var battleData = AIRoot.GetBattleData<BattleData>();
            if (!AllQueues.TryGetValue(type, out var queue))
            {
                return;
            }

            LogHelper.Debug("ApplySlotQueue: " + type.Name);
            battleData.CurrApply = queue;
            battleData.ApplyIndex = 0;
        }

        public async Task<bool> UseSpellQueue()
        {
            var battleData = AIRoot.GetBattleData<BattleData>();
            if (battleData.CurrApply == null)
                return false;
            if (battleData.CurrApply.SlotQueue.Count <= battleData.ApplyIndex)
            {
                ClearApply();
                return false;
            }
            var spellQueue = AIRoot.GetBattleData<SpellQueueData>();
            if (!await spellQueue.ApplySlot())
            {
                var slotProvider = battleData.CurrApply.SlotQueue[battleData.ApplyIndex];
                if (slotProvider.Check(battleData.ApplyIndex) < 0)
                {
                    ClearApply();
                    return false;
                }
                var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
                slotProvider.Fill(slot);
                spellQueue.Add(slot);
                battleData.ApplyIndex++;
            }
            return true;
        }
    }
}