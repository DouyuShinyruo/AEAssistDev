using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_AoERotations : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var needUseAoe = TargetHelper.CheckNeedUseAOE(4, 8);
            if (!needUseAoe)
            {
                return -1;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // AoERotations

            var spell = SamuraiSpellHelper.AoEGCD();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}