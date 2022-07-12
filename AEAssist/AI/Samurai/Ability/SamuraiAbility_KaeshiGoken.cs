using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_KaeshiGoken : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.KaeshiGoken.IsReady()) return -1;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.KaeshiGoken.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}