using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_HissatsuGyoten : IAIHandler
    {

        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.HissatsuGyoten.IsReady()) return -1;
            if (ActionResourceManager.Samurai.Kenki < 10) return -1;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuGyoten.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}