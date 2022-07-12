using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Reaper
{
    [Opener(ClassJobType.Reaper, 90)]
    public class Opener_Reaper_90 : IOpener
    {
        public int Check()
        {
            if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
                return -5;
            if (!AEAssist.DataBinding.Instance.Burst)
                return -100;
            if (!SpellsDefine.ArcaneCircle.IsReady())
                return -1;
            if (!SpellsDefine.SoulSlice.IsMaxChargeReady(0.1f))
                return -2;
            if (!SpellsDefine.Gluttony.CoolDownInGCDs(7))
                return -3;
            if (!SpellsDefine.Enshroud.IsReady())
                return -4;
            return 0;
        }

        public List<Action<SpellQueueSlot>> Openers { get; } = new List<Action<SpellQueueSlot>>()
        {
            Step0,
            Step1,
            Step2,
            Step3
        };


        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCD(ReaperSpellHelper.GetShadowOfDeath().Id, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.ArcaneCircle, SpellTargetType.Self));
        }


        private static void Step1(SpellQueueSlot slot)
        {
            var id = ReaperSpellHelper.CanUseSoulSlice_Scythe(Core.Me.CurrentTarget).Id;
            slot.SetGCD(id, SpellTargetType.CurrTarget);
        }


        private static void Step2(SpellQueueSlot slot)
        {
            var id = ReaperSpellHelper.CanUseSoulSlice_Scythe(Core.Me.CurrentTarget).Id;
            slot.SetGCD(id, SpellTargetType.CurrTarget);
            slot.UsePotion = true;
        }


        private static void Step3(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.PlentifulHarvest, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Enshroud, SpellTargetType.Self));
        }
    }
}