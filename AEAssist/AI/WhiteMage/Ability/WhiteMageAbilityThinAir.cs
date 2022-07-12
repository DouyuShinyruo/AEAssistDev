using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
namespace AEAssist.AI.WhiteMage.Ability
{
    internal class WhiteMageAbilityThinAir : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.ThinAir.IsReady()) return -1;
            if (!SpellsDefine.ThinAir.IsMaxChargeReady())
            {
                return -1;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.ThinAir.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
