using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI
{
    public class SpellQueueSlot : Entity
    {
        // spellId == 0, mean wait {AnimationLockMs}
        public Queue<(uint spellId, SpellTargetType SpellTargetType)> Abilitys =
            new Queue<(uint spellId, SpellTargetType SpellTargetType)>();

        public int AnimationLockMs = 0;
        private uint GCDSpellId;

        public SpellTargetType SpellTargetType;
        public BattleCharacter BattleCharacter;

        public bool UsePotion;

        public int Index;

        public bool GCDQueueMode;
        public Queue<(uint spellId, SpellTargetType targetType)> GCDQueues;
        public int GCDQueueIndex;

        public Func<int> BreakCond;

        public void SetGCD(uint spellId, SpellTargetType targetType)
        {
            this.ClearGCD();
            this.GCDSpellId = spellId;
            this.SpellTargetType = targetType;
        }

        public void SetGCD(uint spellId, BattleCharacter target)
        {
            this.ClearGCD();
            this.GCDSpellId = spellId;
            this.SpellTargetType = SpellTargetType.SpecifyTarget;
            this.BattleCharacter = target;
        }

        public void SetGCDQueue(params (uint spellId, SpellTargetType targetType)[] queues)
        {
            this.ClearGCD();
            GCDQueueMode = true;
            GCDQueues = new Queue<(uint spellId, SpellTargetType targetType)>();
            foreach (var v in queues)
            {
                GCDQueues.Enqueue(v);
            }
        }
        public void SetGCDQueueFromList(List<(uint spellId, SpellTargetType targetType)> queues)
        {
            this.ClearGCD();
            GCDQueueMode = true;
            GCDQueues = new Queue<(uint spellId, SpellTargetType targetType)>();
            foreach (var v in queues)
            {
                GCDQueues.Enqueue(v);
            }
        }

        public void EnqueueGCD((uint spellId, SpellTargetType targetType) value)
        {
            if (!GCDQueueMode)
            {
                this.ClearGCD();
                GCDQueueMode = true;
                GCDQueues = new Queue<(uint spellId, SpellTargetType targetType)>();
            }
            GCDQueues.Enqueue(value);
        }

        public void SetBreakCondition(Func<int> Check)
        {
            BreakCond = Check;
        }

        public void ClearGCD()
        {
            this.GCDSpellId = 0;
            this.GCDQueueMode = false;
            this.GCDQueues = null;
            this.BattleCharacter = null;
            this.GCDQueueIndex = 0;
        }

        public void EnqueueAbility((uint spellId, SpellTargetType spellTargetType) va)
        {
            this.Abilitys.Enqueue(va);
        }

        public uint GetGCDSpell()
        {
            return this.GCDSpellId;
        }

        protected override void OnDestroy()
        {
            ClearGCD();
            Abilitys.Clear();
            UsePotion = false;
            Index = 0;
            AnimationLockMs = 0;
            SpellTargetType = SpellTargetType.CurrTarget;
            BreakCond = null;
        }
    }

    public class SpellQueueData : IBattleData
    {
        private bool isLock;
        public Queue<SpellQueueSlot> Queue = new Queue<SpellQueueSlot>();

        public void Add(SpellQueueSlot slot)
        {
            Queue.Enqueue(slot);
        }

        public void Clear()
        {
            while (Queue.Count > 0)
            {
                var val = Queue.Dequeue();
                ObjectPool.Instance.Return(val);
            }
        }

        public async Task<bool> ApplySlot()
        {
            if (Queue.Count == 0)
                return false;
            if (isLock)
            {
                var slot = Queue.Peek();
                if (slot.BreakCond != null && slot.BreakCond.Invoke() < 0)
                {
                    ObjectPool.Instance.Return(Queue.Dequeue());
                    return await ApplySlot();
                }
                LogHelper.Debug(
                    $"Slot Abilitys: {slot.Abilitys.Count} UsePotion: {slot.UsePotion}");
                if (slot.Abilitys.Count == 0)
                {
                    if (slot.UsePotion)
                    {
                        await AIMgrs.Instance.UsePotion(Core.Me.CurrentJob);
                        isLock = false;
                        ObjectPool.Instance.Return(Queue.Dequeue());
                        return await ApplySlot();
                    }

                    isLock = false;
                    ObjectPool.Instance.Return(Queue.Dequeue());
                    return await ApplySlot();
                }

                var abilityId = slot.Abilitys.Peek();
                var spellData = SpellEntity.Create(abilityId.spellId);
                if (abilityId.spellId == 0 || spellData.SpellData == null || !spellData.IsReady())
                {
                    if (slot.AnimationLockMs > 0)
                        await Coroutine.Sleep(slot.AnimationLockMs);
                    slot.Abilitys.Dequeue();
                }
                else
                {
                    var spellEntity = new SpellEntity(spellData.Id, slot.SpellTargetType);

                    if (await spellEntity.DoAbility())
                    {
                        slot.Abilitys.Dequeue();
                        AIRoot.Instance.RecordAbility(spellData);
                    }
                }

                return true;
            }
            else
            {
                var slot = Queue.Peek();

                if (slot.BreakCond != null && slot.BreakCond.Invoke() < 0)
                {
                    ObjectPool.Instance.Return(Queue.Dequeue());
                    return await ApplySlot();
                }

                if (!slot.GCDQueueMode)
                {
                    var spellData = DataManager.GetSpellData(slot.GetGCDSpell());
                    if (slot.GetGCDSpell() == 0 || spellData == null)
                    {
                        isLock = true;
                        return true;
                    }

                    LogHelper.Debug(
                        $"Slot GCD: {spellData.LocalizedName} Ready: {spellData.IsReady()} {AIRoot.Instance.CanUseGCD()}");

                    if (spellData.IsReady() && AIRoot.Instance.CanUseGCD())
                    {
                        SpellEntity spellEntity = null;
                        if (slot.SpellTargetType == SpellTargetType.SpecifyTarget)
                        {
                            spellEntity = new SpellEntity(spellData.Id, slot.BattleCharacter);
                        }
                        else
                        {
                            spellEntity = new SpellEntity(spellData.Id, slot.SpellTargetType);
                        }

                        if (await spellEntity.DoGCD())
                        {
                            AIRoot.Instance.RecordGCD(spellEntity);
                            isLock = true;
                        }
                    }

                    return true;
                }
                else
                {
                    if (slot.GCDQueues == null || slot.GCDQueues.Count == 0)
                    {
                        isLock = true;
                        return true;
                    }

                    var gcdSpellId = slot.GCDQueues.Peek();
                    var spellData = DataManager.GetSpellData(gcdSpellId.spellId);
                    LogHelper.Debug(
                        $"Slot GCD: {spellData.LocalizedName} Ready: {spellData.IsReady()} {AIRoot.Instance.CanUseGCD()}");
                    if (spellData.IsReady() && AIRoot.Instance.CanUseGCD())
                    {
                        SpellEntity spellEntity = null;
                        spellEntity = new SpellEntity(spellData.Id, gcdSpellId.targetType);
                        if (await spellEntity.DoGCD())
                        {
                            AIRoot.Instance.RecordGCD(spellEntity);
                            slot.GCDQueues.Dequeue();
                        }
                    }

                    return true;
                }
            }
        }
    }
}