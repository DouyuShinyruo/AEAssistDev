using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_StarfallDance : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (!SpellsDefine.StarfallDance.IsUnlock())
            {
                return -10;
            }

            if (!Core.Me.HasAura(AurasDefine.FlourishingStarfall))
            {
                return -1;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.StarfallDance.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}