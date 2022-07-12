using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.GCD
{
    public class GunBreakerGCD_SonicBreak : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.SonicBreak.IsReady())
                return -1;
            if (SpellsDefine.NoMercy.CoolDownInGCDs(4))
                return -2;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.SonicBreak.GetSpellEntity();
            if (await spell.DoGCD())
                return spell;
            return null;
        }
    }
}