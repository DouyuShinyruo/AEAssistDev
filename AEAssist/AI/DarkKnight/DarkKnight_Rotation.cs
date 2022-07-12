using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;

namespace AEAssist.AI.DarkKnight
{
    [Job(ClassJobType.DarkKnight)]
    public class DarkKnight_Rotation : IRotation
    {
        public void Init()
        {
            //CountDownHandler.Instance.AddListener(1500,
            //    () => PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId));


            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<DarkKnightSettings>().EarlyDecisionMode;
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