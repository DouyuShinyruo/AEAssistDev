using AEAssist.AI.BlackMage.SpellQueue;
using AEAssist.Define;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMage_BaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            AISpellQueueMgr.Instance.Apply<SpellQueue_DespairCombo>();
            await Task.CompletedTask;
            return null;
        }
    }
}