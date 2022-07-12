using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_Barrage : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Barrage.IsReady()) return -1;

            if (BardSpellHelper.UnlockBuffsCount() > 1 && BardSpellHelper.HasBuffsCount() <= 1)
                return -3;

            if (Core.Me.HasAura(AurasDefine.ShadowBiteReady)
                && TargetHelper.CheckNeedUseAOE(25, 5, ConstValue.BardAOECount))
                return 1;

            var burstShot = BardSpellHelper.GetHeavyShot();

            var buff = Core.Me.GetAuraById(AurasDefine.RagingStrikes);
            if (buff != null && buff.TimespanLeft.TotalMilliseconds < 7000)
            {
                return 2;
            }

            if (AIRoot.GetBattleData<BattleData>().lastGCDSpell == burstShot
                && !AIRoot.Instance.Is2ndAbilityTime())
                return -4;

            if (Core.Me.HasAura(AurasDefine.StraighterShot))
                return -2;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Barrage.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}