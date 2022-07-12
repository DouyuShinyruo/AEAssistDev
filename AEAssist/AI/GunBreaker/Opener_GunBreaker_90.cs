using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot.Enums;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Monk
{
    [Opener(ClassJobType.Gunbreaker, 90)]
    public class Opener_GunBreaker_90 : IOpener
    {
        public int Check()
        {
            // if (PartyManager.NumMembers <= 4)
            //     return -5;
            if (!AEAssist.DataBinding.Instance.Burst)
                return -100;
            if (!SpellsDefine.NoMercy.IsReady())
                return -4;
            if (!SpellsDefine.GnashingFang.IsReady())
                return -5;
            if (!SpellsDefine.DoubleDown.IsReady())
                return -6;
            if (!SpellsDefine.Bloodfest.IsReady())
                return -7;
            if (!SpellsDefine.RoughDivide.IsMaxChargeReady())
                return -8;
            return 0;
        }

        public List<Action<SpellQueueSlot>> Openers { get; } = new List<Action<SpellQueueSlot>>()
        {
            //StepPre,
            Step0,
            Step1,
            Step2,
            Step3,
            Step4,
            Step5,
            // Step6
        };

        private static void StepPre(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.LightningShot, SpellTargetType.CurrTarget);
            slot.UsePotion = true;
        }

        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.KeenEdge, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Bloodfest, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.NoMercy, SpellTargetType.Self));
        }


        private static void Step1(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.GnashingFang, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.BlastingZone, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.JugularRip, SpellTargetType.CurrTarget));
        }


        private static void Step2(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.SonicBreak, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.BowShock, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.RoughDivide, SpellTargetType.CurrTarget));
        }


        private static void Step3(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.DoubleDown, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.RoughDivide, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Hypervelocity, SpellTargetType.CurrTarget));
        }


        private static void Step4(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.SavageClaw, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.AbdomenTear, SpellTargetType.CurrTarget));
        }
        private static void Step5(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.WickedTalon, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.EyeGouge, SpellTargetType.CurrTarget));
        }
    }
}