using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.Ability
{
    public class DancerAbility_Flourish : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Flourish.IsUnlock())
            {
                return -10;
            }

            if (!SpellsDefine.Flourish.IsReady())
            {
                return -1;
            }

            if (!AEAssist.DataBinding.Instance.UseFlourish)
            {
                return -3;
            }

            if (SpellsDefine.FanDance.RecentlyUsed())
            {
                return -2;
            }

            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;

            if (Core.Me.HasAura(AurasDefine.FlourshingFlow) && (bdls != SpellsDefine.Fountainfall.GetSpellEntity() &&
                                                                bdls != SpellsDefine.Bloodshower.GetSpellEntity()))
            {
                return -2;
            }

            if (Core.Me.HasAura(AurasDefine.FlourishingSymmetry) && (bdls != SpellsDefine.ReverseCascade.GetSpellEntity() &&
                                                                     bdls != SpellsDefine.RisingWindmill.GetSpellEntity()))
            {
                return -2;
            }

            SpellEntity[] GCDsCanProc =
            {
                SpellsDefine.Cascade.GetSpellEntity(),
                SpellsDefine.Fountain.GetSpellEntity(),
                SpellsDefine.Windmill.GetSpellEntity(),
                SpellsDefine.Bladeshower.GetSpellEntity(),
            };

            if (GCDsCanProc.Contains(bdls))
            {
                if (!AIRoot.Instance.Is2ndAbilityTime())
                {
                    return -1;
                }
            }

            if (Core.Me.HasMyAura(AurasDefine.ThreeFoldFanDance) && !SpellsDefine.FanDance3.RecentlyUsed())
            {
                return -2;
            }

            return 0;

        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Flourish.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}