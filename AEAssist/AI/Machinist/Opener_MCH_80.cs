using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Machinist
{
    [Opener(ClassJobType.Machinist, 80)]
    public class Opener_MCH_80 : IOpener
    {
        public int Check()
        {
            if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
                return -5;
            if (ActionResourceManager.Machinist.Heat >= 50)
                return -1;
            if (ActionResourceManager.Machinist.Battery >= 50)
                return -2;
            if (ActionResourceManager.Machinist.OverheatRemaining.TotalMilliseconds > 0)
                return -3;
            if (!SpellsDefine.BarrelStabilizer.IsReady())
                return -4;
            if (!SpellsDefine.AutomationQueen.IsReady())
                return -6;
            if (!SpellsDefine.Hypercharge.CoolDownInGCDs(4))
                return -7;
            if (!SpellsDefine.Wildfire.CoolDownInGCDs(3))
                return -8;
            return 0;
        }

        public List<Action<SpellQueueSlot>> Openers { get; } = new List<Action<SpellQueueSlot>>()
        {
            Step0,
            Step1,
            Step2,
            Step3,
            Step4
        };


        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.AirAnchor, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.GaussRound, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Ricochet, SpellTargetType.CurrTarget));
        }


        private static void Step1(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.Drill, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.BarrelStabilizer, SpellTargetType.CurrTarget));
        }


        private static void Step2(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.SplitShot, SpellTargetType.CurrTarget);
        }


        private static void Step3(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.SlugShot, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.GaussRound, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Ricochet, SpellTargetType.CurrTarget));
        }


        private static void Step4(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.HeatedCleanShot, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Hypercharge, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Wildfire, SpellTargetType.CurrTarget));
        }
    }
}