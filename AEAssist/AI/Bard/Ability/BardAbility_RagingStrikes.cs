using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_RagingStrikes : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.Instance.CloseBurst)
                return -1;
            if (!SpellsDefine.RagingStrikes.IsReady())
                return -2;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -3;
            if (SpellsDefine.TheWanderersMinuet.IsUnlock())
            {
                if (AIRoot.GetBattleData<BattleData>().CurrBattleTimeInMs < 10000)
                {
                    if (SpellsDefine.TheWanderersMinuet.RecentlyUsed()
                        || Core.Me.HasMyAuraWithTimeleft(AurasDefine.TheWanderersMinuet))
                        return 0;
                    return -4;
                }

                if (!SpellsDefine.TheWanderersMinuet.RecentlyUsed()
                    && !Core.Me.HasMyAuraWithTimeleft(AurasDefine.TheWanderersMinuet))
                    return -5;
                if (AIRoot.GetBattleData<BattleData>().lastGCDIndex
                    - SpellHistoryHelper.GetLastGCDIndex(SpellsDefine.TheWanderersMinuet) >= 1)
                    return 1;
                return -6;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // if (!AIRoot.Instance.Is2ndAbilityTime())
            //     return null;
            var SpellEntity = SpellsDefine.RagingStrikes.GetSpellEntity();
            if (await SpellEntity.DoAbility()) return SpellEntity;

            return null;
        }
    }
}