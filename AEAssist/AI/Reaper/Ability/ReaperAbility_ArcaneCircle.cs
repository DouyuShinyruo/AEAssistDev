using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.Ability
{
    public class ReaperAbility_ArcaneCircle : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.ArcaneCircle.IsReady())
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (!Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget))
                return -3;
            if (AEAssist.SettingMgr.GetSetting<ReaperSettings>().DoubleEnshroudPrefer)
            {
                if (!SpellsDefine.PlentifulHarvest.IsUnlock())
                    return 1;
                if (!SpellsDefine.Enshroud.IsUnlock())
                    return -7;
                if ((SpellsDefine.PlentifulHarvest.RecentlyUsed() || ActionResourceManager.Reaper.ShroudGauge >= 50) &&
                    !Core.Me.HasAura(AurasDefine.Enshrouded))
                    return -4;
                if (SpellsDefine.Enshroud.RecentlyUsed() || Core.Me.HasAura(AurasDefine.Enshrouded))
                {
                    var delta = AIRoot.GetBattleData<BattleData>().lastGCDIndex -
                                SpellHistoryHelper.GetLastGCDIndex(SpellsDefine.Enshroud);
                    if (delta < 2)
                        return -5;
                    if (!AIRoot.Instance.Is2ndAbilityTime(0.6f))
                        return -6;
                }
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.ArcaneCircle.DoAbility()) return SpellsDefine.ArcaneCircle.GetSpellEntity();

            return null;
        }
    }
}