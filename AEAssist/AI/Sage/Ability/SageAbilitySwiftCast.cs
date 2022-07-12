using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.Ability
{
    public class SageAbilitySwiftCast : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Swiftcast.IsReady()) return -1;
            if (!SpellsDefine.Egeiro.IsReady()) return -1;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Swiftcast.GetSpellEntity();
            if (spell == null)
            {
                return null;
            }
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}