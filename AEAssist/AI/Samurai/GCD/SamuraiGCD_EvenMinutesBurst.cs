using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_EvenMinutesBurst : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var bd = AIRoot.GetBattleData<SamuraiBattleData>();
            if (bd.CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                return 0;
            }

            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SamuraiSpellHelper.EvenMinutesBurst();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}