using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage
{
    [Job(ClassJobType.WhiteMage)]
    internal class WhiteMageRotation : IRotation
    {
        public void Init()
        {
            //CountDownHandler.Instance.AddListener(15000, WhiteMageSpellHelper.PrePullEukrasianDiagnosisThreePeople);
            CountDownHandler.Instance.AddListener(2500, () =>
            PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));
            CountDownHandler.Instance.AddListener(1500, () => WhiteMageSpellHelper.GetStone().DoGCD());
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<WhiteMageSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
        }
        public Task<bool> PreCombatBuff()
        {
            return Task.FromResult(false);
        }

        public Task<bool> NoTarget()
        {
            return Task.FromResult(false);
        }

        public SpellEntity GetBaseGCDSpell()
        {
            return WhiteMageSpellHelper.GetBaseGcd();
        }
    }
}

