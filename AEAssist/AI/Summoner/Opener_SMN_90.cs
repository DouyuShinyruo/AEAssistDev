using AEAssist.AI.Summoner.Ability;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Summoner
{
    [Opener(ClassJobType.Summoner, 90)]
    public class Opener_SMN_90 : IOpener
    {
        public int Check()
        {
            if (!SMN_SpellHelper.HasCarbuncle())
                return -4;

            if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
                return -5;
            
            if (!SpellsDefine.SummonBahamut.IsReady())
                return -6;

            if (SMN_SpellHelper.PhoenixTrance())
                return -7;

            return 0;
        }

        public List<Action<SpellQueueSlot>> Openers { get; } = new List<Action<SpellQueueSlot>>()
        {
          pre1,
          pre2,
          Step0,
        };

        public int StepCount => 3;

        private static void pre1(SpellQueueSlot slot) 
        { 
            if (AIRoot.GetBattleData<BattleData>().lastGCDSpell == null || DataBinding.Instance.SMNSettings.DelayOpeningBurst)
                slot.SetGCD(SpellsDefine.Ruin3, SpellTargetType.CurrTarget);
        }
        private static void pre2(SpellQueueSlot slot)
        {
            if (SpellsDefine.SearingLight.IsReady())
                slot.SetGCD(SpellsDefine.SearingLight, SpellTargetType.Self);
        }
        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.SummonBahamut, SpellTargetType.CurrTarget);
            slot.UsePotion = true;
        }

        private static void Step1(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.AstralImpulse, SpellTargetType.CurrTarget);
            uint spell = SMNAbility_Fester.GetSpell();
            if ((ActionResourceManager.Summoner.Aetherflow == 2))
            {
                slot.EnqueueAbility((spell, SpellTargetType.CurrTarget));
                slot.EnqueueAbility((spell, SpellTargetType.CurrTarget));
                return;
            }
            else if ((ActionResourceManager.Summoner.Aetherflow == 1))
            {
                slot.EnqueueAbility((spell, SpellTargetType.CurrTarget));
            }

            slot.EnqueueAbility((SMNAbility_Fester.GetSpell(), SpellTargetType.CurrTarget));
        }
        private static void Step2(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.AstralImpulse, SpellTargetType.CurrTarget);
            slot.EnqueueAbility((SpellsDefine.Fester, SpellTargetType.CurrTarget));

        }

    }
}