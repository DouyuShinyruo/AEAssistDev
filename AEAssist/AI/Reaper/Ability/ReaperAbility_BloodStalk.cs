using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.Ability
{
    public class ReaperAbility_BloodStalk : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.BloodStalk.IsUnlock())
                return -1;
            if (!AEAssist.DataBinding.Instance.UseSoulGauge)
                return -2;

            if (AIRoot.Instance.CloseBurst && !SpellsDefine.Enshroud.IsUnlock())
                return -10;

            if (ActionResourceManager.Reaper.SoulGauge < 50)
                return -3;
            if (ReaperSpellHelper.IfHasSoulReaver())
                return -4;
            if (SpellsDefine.Enshroud.GetSpellEntity().Cooldown.TotalMilliseconds > 0
                && SpellsDefine.Enshroud.GetSpellEntity().Cooldown.TotalMilliseconds < 5000
                && ReaperSpellHelper.ReadyToEnshroud() >= 0)
                return -5;

            if (TimeHelper.Now() - SpellHistoryHelper.GetLastSpellTime(SpellsDefine.Enshroud) < 3000)
                return -6;
            if (SpellsDefine.Enshroud.RecentlyUsed() || Core.Me.HasAura(AurasDefine.Enshrouded))
                return -7;
            if (SpellsDefine.Gluttony.IsUnlock()
                && SpellsDefine.Gluttony.GetSpellEntity().Cooldown.TotalMilliseconds < 10000 && ActionResourceManager.Reaper.SoulGauge < 90) return -8;

            if (ReaperSpellHelper.CheckCanUsePlentifulHarvest() >= 0)
            {
                return -9;
            }

            if (ReaperSpellHelper.ReadyToEnshroud() >= 0)
            {
                return -10;
            }

            if (ActionManager.ComboTimeLeft > 0 && ActionManager.ComboTimeLeft < 3.5f)
                return -11;

            if (!Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget))
                return -12;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.BloodStalk;
            if (SpellsDefine.GrimSwathe.IsUnlock() && TargetHelper.CheckNeedUseAOE(8, 8))
                spell = SpellsDefine.GrimSwathe;

            if (await spell.DoAbility())
                // if (AIRoot.GetBattleData<BattleData>().maxAbilityTimes>1 && await ReaperSpellHelper.UseTruthNorth() != null)
                // {
                //     if (AIRoot.GetBattleData<BattleData>().maxAbilityTimes > 1)
                //         AIRoot.Instance.MuteAbilityTime();
                // }

                return spell.GetSpellEntity();

            return null;
        }
    }
}