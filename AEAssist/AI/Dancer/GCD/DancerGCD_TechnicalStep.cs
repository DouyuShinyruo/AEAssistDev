using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_TechnicalStep : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (!SpellsDefine.TechnicalStep.IsUnlock())
            {
                return -10;
            }
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.TechnicalStep.IsReady())
            {
                return -1;
            }
            if (SettingMgr.GetSetting<DancerSettings>().UseDanceOnlyInRange)
            {
                if (Core.Me.CurrentTarget.Distance(Core.Me) > 15)
                {
                    return -1;
                }
            }

            if (Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                return -2;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // Cascade 瀑泻 ST1 Reverse Cascade 逆瀑泻
            // Fountain 喷泉 ST2 :Fountainfall 坠喷泉 
            // Windmill 风车 AOE1 Rising Windmill 升风车 
            // Bladeshower 落刃雨 AOE2 :Bloodshower 落血雨 
            var spell = SpellsDefine.TechnicalStep.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}