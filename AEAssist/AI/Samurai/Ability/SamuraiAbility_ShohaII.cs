using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_ShohaII : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.ShohaII.IsReady()) return -1;
            if (ActionResourceManager.Samurai.Meditation != 3) return -2;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.ShohaII.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}