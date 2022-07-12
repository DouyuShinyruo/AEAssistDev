using AEAssist.AI.Dancer.SpellQueue;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_DanceStep : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            if (bdls == SpellsDefine.DoubleStandardFinish.GetSpellEntity() ||
                bdls == SpellsDefine.QuadrupleTechnicalFinish.GetSpellEntity() ||
                (!Core.Me.HasAura(AurasDefine.StandardStep) && !Core.Me.HasAura(AurasDefine.TechnicalStep))
               )
            {
                return -10;
            }

            if (ActionResourceManager.Dancer.CurrentStep == ActionResourceManager.Dancer.DanceStep.Finish)
            {
                return -5;
            }
            if (bdls == SpellsDefine.StandardStep.GetSpellEntity() ||
                bdls == SpellsDefine.TechnicalStep.GetSpellEntity() ||
                Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                return 0;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            // Cascade 瀑泻 ST1 Reverse Cascade 逆瀑泻
            // Fountain 喷泉 ST2 :Fountainfall 坠喷泉 
            // Windmill 风车 AOE1 Rising Windmill 升风车 
            // Bladeshower 落刃雨 AOE2 :Bloodshower 落血雨 
            try
            {
                if (ActionResourceManager.Dancer.Steps.Length > 2)
                {
                    foreach (var step in ActionResourceManager.Dancer.Steps)
                    {
                        LogHelper.Error(step.ToString());
                    }
                    AISpellQueueMgr.Instance.Apply<SpellQueue_Dance>();
                    await Task.CompletedTask;
                    return null;
                }
            }
            catch
            {
                // ignored
            }

            return null;

        }
    }
}