using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.GCD
{
    public class MCHGCD_ReassembleGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var spell = MCHSpellHelper.GetReassembleGCD();
            if (spell == null)
                return -1;

            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.Reassembled) || SpellsDefine.Reassemble.RecentlyUsed()) return 1;

            return -2;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = MCHSpellHelper.GetReassembleGCD();
            if (spell == null)
                return null;
            if (await spell.DoGCD()) return spell;

            return null;
        }
    }
}