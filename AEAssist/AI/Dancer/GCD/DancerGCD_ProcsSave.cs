using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_ProcsSave : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (!SpellsDefine.ReverseCascade.IsUnlock())
            {
                return -10;
            }
            if (!Core.Me.HasAura(AurasDefine.FlourishingSymmetry) &&
                !Core.Me.HasAura(AurasDefine.FlourshingFlow))
            {
                return -1;
            }

            // if (ActionResourceManager.Dancer.FourFoldFeathers == 4)
            // {
            //     return -2;
            // }
            // standered stance 4500
            // if tech is coming soon
            // if combo will drop after tech 
            // if combo will drop after standered stance
            if (Core.Me.HasAura(AurasDefine.FlourishingSymmetry) &&
                !Core.Me.HasMyAuraWithTimeleft(AurasDefine.FlourishingSymmetry, 5000))
            {
                return 1;
            }
            if (Core.Me.HasAura(AurasDefine.FlourshingFlow) &&
                !Core.Me.HasMyAuraWithTimeleft(AurasDefine.FlourshingFlow, 5000))
            {
                return 1;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.ReverseCascade.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.FlourshingFlow))
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 2))
                {
                    spell = SpellsDefine.Bloodshower.GetSpellEntity();
                }
                else
                {
                    spell = SpellsDefine.Fountainfall.GetSpellEntity();
                }
            }
            else
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 2))
                {
                    spell = SpellsDefine.RisingWindmill.GetSpellEntity();
                }
            }
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}