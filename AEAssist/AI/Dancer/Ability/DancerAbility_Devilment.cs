using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.Ability
{
    public class DancerAbility_Devilment : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Devilment.IsUnlock())
            {
                return -10;
            }
            if (!SpellsDefine.Devilment.IsReady())
            {
                return -10;
            }
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            if (bdls == SpellsDefine.QuadrupleTechnicalFinish.GetSpellEntity() || Core.Me.HasAura(AurasDefine.TechnicalFinish))
            {
                return 0;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Devilment.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}