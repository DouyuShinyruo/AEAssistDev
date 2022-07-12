using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot.Enums;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja
{
    [Job(ClassJobType.Ninja)]
    public class NinjaRotation : IRotation
    {
        public void Init()
        {
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<NinjaSettings>().EarlyDecisionMode;
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
            return SpellsDefine.SpinningEdge.GetSpellEntity();
        }
    }
}