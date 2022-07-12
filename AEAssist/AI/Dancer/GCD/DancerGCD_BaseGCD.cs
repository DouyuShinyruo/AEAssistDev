using AEAssist.Define;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_BaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                return -10;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // Cascade 瀑泻 ST1 Reverse Cascade 逆瀑泻
            // Fountain 喷泉 ST2 :Fountainfall 坠喷泉 
            // Windmill 风车 AOE1 Rising Windmill 升风车 
            // Bladeshower 落刃雨 AOE2 :Bloodshower 落血雨 

            return await DancerSpellHelper.BaseGCDCombo(Core.Me.CurrentTarget);
        }
    }
}