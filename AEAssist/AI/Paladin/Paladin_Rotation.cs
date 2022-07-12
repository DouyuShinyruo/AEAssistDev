using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.Paladin
{
    [Job(ClassJobType.Paladin)]
    public class Paladin_Rotation : IRotation
    {
        public void Init()
        {
            //CountDownHandler.Instance.AddListener(1500,
            //    () => PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));


            AEAssist.DataBinding.Instance.EarlyDecisionMode = DataBinding.Instance.PaladinSettings.EarlyDecisionMode;
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
            return SpellsDefine.FastBlade.GetSpellEntity();
        }
    }
}