using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_OddMinuteBurst : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var bd = AIRoot.GetBattleData<SamuraiBattleData>();
            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                return -1;
            }

            if (AIRoot.GetBattleData<SamuraiBattleData>().higanBanaCount < 1)
            {
                if (SamuraiSpellHelper.SenCounts() == 1)
                {
                    return -1;
                }   
            }

            if (bd.CurrPhase != SamuraiPhase.OddMinutesBurstPhase)
            {
                return -1;
            }

            
            LogHelper.Info("We are in OddBurst now");
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SamuraiSpellHelper.OddMinutesBurst();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
            {
                AIRoot.GetBattleData<SamuraiBattleData>().higanBanaCount++;
                return spell;
            }
            return null;
        }
    }
}