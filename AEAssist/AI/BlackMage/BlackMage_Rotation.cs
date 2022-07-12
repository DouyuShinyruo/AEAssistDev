using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage
{
    [Job(ClassJobType.BlackMage)]
    public class BlackMage_Rotation : IRotation
    {
        public void Init()
        {
            CountDownHandler.Instance.AddListener(21000, () => SpellsDefine.Sharpcast.DoAbility());
            CountDownHandler.Instance.AddListener(3500, () => SpellsDefine.Fire3.DoAbility());
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<BlackMageSetting>().EarlyDecisionMode;
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
            return SpellsDefine.Fire.GetSpellEntity();
        }
    }
}