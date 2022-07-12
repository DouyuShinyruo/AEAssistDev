using AEAssist.AI.Summoner.GCD;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner
{
    [Job(ClassJobType.Summoner)]
    public class SMN_Rotation : IRotation
    {
        public void Init()
        {
            //CountDownHandler.Instance.AddListener(1500,
            //    () => PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));
            int castTime = (int)SpellsDefine.Ruin3.GetSpellEntity().SpellData.AdjustedCastTime.TotalMilliseconds;
            int randomTimer = new Random().Next(castTime-500, castTime);
            CountDownHandler.Instance.AddListener(randomTimer,
                async () => await SMN_SpellHelper.CountDownOpener());

            DataBinding.Instance.EarlyDecisionMode = DataBinding.Instance.SMNSettings.EarlyDecisionMode;
        }
        public async Task<bool> PreCombatBuff()
        {
            var summonCarbuncle = new SMNGCD_SummonCarbuncle();
            if (summonCarbuncle.Check(null) >= 0 && !Core.Me.IsMounted)
            {
                await summonCarbuncle.DelayedRun();
                return true;
            }


            return false;

        }

        public Task<bool> NoTarget()
        {

            return Task.FromResult(false);
        }

        public SpellEntity GetBaseGCDSpell()
        {
            return SMNGCD_Base.GetSpell().GetSpellEntity();
        }
    }
}