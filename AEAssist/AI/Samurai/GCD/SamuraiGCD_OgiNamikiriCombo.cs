using AEAssist.AI.Samurai.SpellQueue;
using AEAssist.Define;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_OgiNamikiriCombo : IAIHandler
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
            AISpellQueueMgr.Instance.Apply<SpellQueue_OgiNamikiriCombo>();
            await Task.CompletedTask;
            return null;
        }
        
    }
}