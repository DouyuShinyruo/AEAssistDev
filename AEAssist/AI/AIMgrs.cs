using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI
{
    public interface IAIHandler
    {
        int Check(SpellEntity lastSpell);

        Task<SpellEntity> Run();
    }
    public interface IAIPriorityQueue
    {
        List<IAIHandler> GCDQueue { get; }

        List<IAIHandler> AbilityQueue { get; }

        Task<bool> UsePotion();
    }

    public class AIMgrs
    {
        public static readonly AIMgrs Instance = new AIMgrs();

        public Dictionary<ClassJobType, IAIPriorityQueue> JobPriorityQueue =
            new Dictionary<ClassJobType, IAIPriorityQueue>();

        public void Init()
        {
            JobPriorityQueue.Clear();
            var baseType = typeof(IAIPriorityQueue);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;
                var attrs = type.GetCustomAttributes(typeof(JobAttribute), false);
                if (attrs.Length == 0)
                {
                    LogHelper.Error($"AIPriorityQueue class [{type}] need JobAttribute");
                    continue;
                }

                var attr = attrs[0] as JobAttribute;
                JobPriorityQueue[attr.ClassJobType] = Activator.CreateInstance(type) as IAIPriorityQueue;
                LogHelper.Debug("Load AIPriorityQueue: " + attr.ClassJobType);
            }
        }

        public async Task<SpellEntity> HandleGCD(ClassJobType classJobType, SpellEntity lastGCD)
        {
            if (!JobPriorityQueue.TryGetValue(classJobType, out var queue)) return null;

            if (lastGCD == null)
                lastGCD = SpellEntity.Default;
            foreach (var v in queue.GCDQueue)
            {
                var ret = v.Check(lastGCD);
                LogHelper.Debug(
                    $"{AIRoot.GetBattleData<BattleData>().CurrBattleTimeInMs / 1000.0f:#0.000}  Check:{v.GetType().Name} ret: {ret}");
                if (ret >= 0)
                {
                    return await v.Run();
                }
            }

            return null;
        }

        public async Task<SpellEntity> HandleAbility(ClassJobType classJobType, SpellEntity lastAbility)
        {
            if (!JobPriorityQueue.TryGetValue(classJobType, out var queue)) return null;
            if (lastAbility == null)
                lastAbility = SpellEntity.Default;
            foreach (var v in queue.AbilityQueue)
            {
                if (AIRoot.GetBattleData<BattleData>().CurrApply != null)
                    return null;
                if (AIRoot.GetBattleData<BattleData>().maxAbilityTimes == 0)
                    return null;
                var ret = v.Check(lastAbility);
                LogHelper.Debug(
                    $"{AIRoot.GetBattleData<BattleData>().CurrBattleTimeInMs / 1000.0f:#0.000}  Check:{v.GetType().Name} ret: {ret}");
                if (ret >= 0)
                {
                    return await v.Run();
                }
            }

            return null;
        }

        public async Task<bool> UsePotion(ClassJobType classJobType)
        {
            if (!JobPriorityQueue.TryGetValue(classJobType, out var queue)) return false;
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return false;
            return await queue.UsePotion();
        }
    }
}