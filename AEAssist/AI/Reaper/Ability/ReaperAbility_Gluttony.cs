using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.Ability
{
    public class ReaperAbility_Gluttony : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Gluttony.IsReady())
                return -1;
            if (AIRoot.Instance.CloseBurst && !SpellsDefine.Enshroud.IsUnlock())
                return -101;
            if (ReaperSpellHelper.IfHasSoulReaver())
                return -4;
            if (SpellsDefine.Enshroud.RecentlyUsed() || Core.Me.HasAura(AurasDefine.Enshrouded))
                return -4;
            if (TimeHelper.Now() - SpellHistoryHelper.GetLastSpellTime(SpellsDefine.Enshroud) < 3000)
                return -5;
            // 死亡祭祀
            if (Core.Me.HasAura(AurasDefine.BloodsownCircle))
                return -6;
            // 死亡祭品
            if (Core.Me.HasAura(AurasDefine.ImmortalSacrifice)) return -7;

            if (ActionResourceManager.Reaper.SoulGauge < 50)
                return -8;

            var ret = ReaperSpellHelper.ReadyToEnshroud();
            if (ret >= 0)
                return -9;
            if (!Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget))
                return -10;

            if (ActionManager.ComboTimeLeft > 0 && ActionManager.ComboTimeLeft < 6)
                return -11;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.Gluttony.DoAbility())
                // if (AIRoot.GetBattleData<BattleData>().maxAbilityTimes>0 && await ReaperSpellHelper.UseTruthNorth() != null)
                // {
                //     if (AIRoot.GetBattleData<BattleData>().maxAbilityTimes > 1)
                //         AIRoot.Instance.MuteAbilityTime();
                // }

                return SpellsDefine.Gluttony.GetSpellEntity();

            return null;
        }
    }
}