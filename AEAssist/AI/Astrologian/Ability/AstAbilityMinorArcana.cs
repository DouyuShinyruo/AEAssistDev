using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityMinorArcana : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            if (!SpellsDefine.MinorArcana.IsReady()) return -1;

            if (Core.Me.HasAura(AurasDefine.LordOfCrownsDrawn) || Core.Me.HasAura(AurasDefine.LadyOfCrownsDrawn))
            {
                LogHelper.Debug("有卡不抽");
                return -2;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.MinorArcana.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
