using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_SaberDanceBurstWindow : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (!SpellsDefine.SaberDance.IsUnlock())
            {
                return -10;
            }
            if (Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                return -2;
            }
            if (ActionResourceManager.Dancer.Esprit < 50)
            {
                return -1;
            }

            if (Core.Me.HasAura(AurasDefine.Devilment))
            {
                return 0;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.SaberDance.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}