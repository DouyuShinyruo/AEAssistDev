using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.GCD
{
    public class ReaperGCD_Gibbit_Gallows : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Gibbet.IsUnlock()) return -1;

            if (Core.Me.HasAura(AurasDefine.SoulReaver))
                return 0;
            return -2;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = ReaperSpellHelper.Gibbit_Gallows(Core.Me.CurrentTarget);
            if (spell == null)
                return null;
            if (await spell.DoGCD()) return spell;

            return null;
        }
    }
}