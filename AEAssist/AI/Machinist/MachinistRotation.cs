using AEAssist.AI.GeneralAI;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist
{
    [Job(ClassJobType.Machinist)]
    public class MachinistRotation : IRotation
    {
        public void Init()
        {
            CountDownHandler.Instance.AddListener(1500,
                () => PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId));

            CountDownHandler.Instance.AddListener(4800,
                () => SpellsDefine.Reassemble.DoAbility());

            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<MCHSettings>().EarlyDecisionMode;
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
            return MCHSpellHelper.GetSplitShot();
        }
    }
}