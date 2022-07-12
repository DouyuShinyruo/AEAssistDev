using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Paladin
{
    [Opener(ClassJobType.Paladin, 90)]
    public class Opener_PLD_90 : IOpener
    {
        public int Check()
        {
            
            if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
                return -5;

            return -1;
            //return 0;
        }

        public List<Action<SpellQueueSlot>> Openers { get; } = new List<Action<SpellQueueSlot>>()
        {
            pre,
            Step0,
            //searing,
            Step1,
            //potion,
            Step2,
            Step3,
            Step4,
        };

        public int StepCount => 6;

        private static void pre(SpellQueueSlot slot)
        {
            if (AIRoot.GetBattleData<BattleData>().lastGCDSpell==null)
                slot.SetGCD(SpellsDefine.Ruin3, SpellTargetType.CurrTarget);

        }

        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.Ruin3, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.SearingLight, SpellTargetType.Self));

        }

        private static void searing(SpellQueueSlot slot)
        {
            slot.GCDQueueMode = false;
            slot.Abilitys.Enqueue((0, SpellTargetType.Self));
            slot.AnimationLockMs = 500;
            slot.Abilitys.Enqueue((SpellsDefine.SearingLight, SpellTargetType.Self));
        }
        private static void Step1(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.SummonBahamut, SpellTargetType.CurrTarget);
            slot.UsePotion = true;

        }

        private static void potion(SpellQueueSlot slot)
        {
            slot.GCDQueueMode = false;
            slot.UsePotion = true;
        }
        private static void Step2(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.AstralFlare, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.EnergyDrain, SpellTargetType.CurrTarget));
        }


        private static void Step3(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.AstralFlare, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Fester, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.EnkindleBahamut, SpellTargetType.CurrTarget));
        }


        private static void Step4(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.AstralFlare, SpellTargetType.CurrTarget);
            slot.Abilitys.Enqueue((SpellsDefine.Fester, SpellTargetType.CurrTarget));
            slot.Abilitys.Enqueue((SpellsDefine.Deathflare, SpellTargetType.CurrTarget));
        }

    }
}