using AEAssist.AI.GeneralAI;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard
{
    [Job(ClassJobType.Bard)]
    public class BardRotation : IRotation
    {
        public void Init()
        {
            BardSpellHelper.Init();
            CountDownHandler.Instance.AddListener(1500, () =>
                PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId));
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<BardSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
        }

        public async Task<bool> PreCombatBuff()
        {
            if (!SettingMgr.GetSetting<BardSettings>().UsePeloton)
            {
                GUIHelper.ShowInfo(Language.Instance.Content_Bard_PreCombat1);
                return false;
            }
            return await PhysicsRangeDPSHelper.UsePoleton();
        }
        public Task<bool> NoTarget()
        {
            return Task.FromResult(false);
        }
        public SpellEntity GetBaseGCDSpell()
        {
            return BardSpellHelper.GetBaseGCD();
        }
    }
}