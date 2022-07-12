using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper
{
    [Job(ClassJobType.Reaper)]
    public class ReaperRotation : IRotation
    {
        private readonly AIRoot AiRoot = AIRoot.Instance;

        public void Init()
        {
            CountDownHandler.Instance.AddListener(1500, () =>
            {
                if (Core.Me.HasTarget && Core.Me.CurrentTarget.CanAttack)
                    return SpellsDefine.Harpe.DoGCD();
                return Task.FromResult(false);
            });
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<ReaperSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
        }

        public async Task<bool> PreCombatBuff()
        {
            if (Core.Me.HasAura(AurasDefine.Soulsow))
                return true;
            if (await SpellsDefine.Soulsow.DoGCD())
            {
                GUIHelper.ShowInfo(Language.Instance.Content_Reaper_PreCombat2, 500, false);
                return true;
            }

            return false;
        }

        public async Task<bool> NoTarget()
        {
            if (MovementManager.IsMoving)
                return false;

            // maybe after revive todo: get the id of revive aura
            if (Core.Me.CurrentHealth * 100 / Core.Me.MaxHealth < 50)
                return false;

            if (Core.Me.HasAura(AurasDefine.Soulsow))
                return true;

            return await SpellsDefine.Soulsow.DoGCD();
        }

        public SpellEntity GetBaseGCDSpell()
        {
            return SpellsDefine.Slice.GetSpellEntity();
        }
    }
}