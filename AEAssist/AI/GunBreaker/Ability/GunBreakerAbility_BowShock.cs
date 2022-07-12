using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.Ability
{
    public class GunBreakerAbility_BowShock : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.BowShock.IsReady())
                return -1;

            if (SpellsDefine.NoMercy.CoolDownInGCDs(4))
                return -2;

            return 0;
        }
        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.BowShock.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}
