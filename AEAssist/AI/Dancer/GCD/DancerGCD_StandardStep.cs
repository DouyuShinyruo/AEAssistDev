using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.GCD
{
    public class DancerGCD_StandardStep : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            if (!SpellsDefine.StandardStep.IsUnlock() && Core.Me.ClassLevel < 15)
            {
                return -10;
            }
            if (!SpellsDefine.StandardStep.IsReady())
            {
                return -1;
            }

            if (Core.Me.HasAura(AurasDefine.StandardStep) ||
                Core.Me.HasAura(AurasDefine.TechnicalStep))
            {
                return -2;
            }

            if (SpellsDefine.Flourish.IsUnlock())
            {
                if (SpellsDefine.Flourish.GetSpellEntity().SpellData.Cooldown < TimeSpan.FromSeconds(4))
                {
                    return -3;
                }
            }
            if (SpellsDefine.TechnicalStep.IsUnlock())
            {
                if (SpellsDefine.TechnicalStep.GetSpellEntity().SpellData.Cooldown < TimeSpan.FromSeconds(4))
                {
                    return -3;
                }
            }
            if (SettingMgr.GetSetting<DancerSettings>().UseDanceOnlyInRange)
            {
                if (Core.Me.CurrentTarget.Distance(Core.Me) > 15)
                {
                    return -1;
                }
            }


            var bd = AIRoot.GetBattleData<BattleData>();
            if (Core.Me.HasAura(AurasDefine.TechnicalFinish) &&
                bd.lastAbilitySpell != SpellsDefine.Flourish.GetSpellEntity() &&
                bd.lastGCDSpell != SpellsDefine.QuadrupleTechnicalFinish.GetSpellEntity())
            {
                if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.TechnicalFinish, 4000) &&
                    !(SpellsDefine.Devilment.GetSpellEntity().SpellData.Cooldown < TimeSpan.FromSeconds(4)))
                {
                    if (!Core.Me.HasAura(AurasDefine.FlourshingFlow) &&
                        !Core.Me.HasAura(AurasDefine.FlourishingSymmetry) &&
                        ActionResourceManager.Dancer.Esprit < 50 &&
                        !Core.Me.HasAura(AurasDefine.FlourishingFinish) &&
                        ActionResourceManager.Dancer.FourFoldFeathers == 0 &&
                        !Core.Me.HasMyAura(AurasDefine.ThreeFoldFanDance))
                    {
                        return 1;
                    }
                }

                return -5;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // Cascade 瀑泻 ST1 Reverse Cascade 逆瀑泻
            // Fountain 喷泉 ST2 :Fountainfall 坠喷泉 
            // Windmill 风车 AOE1 Rising Windmill 升风车 
            // Bladeshower 落刃雨 AOE2 :Bloodshower 落血雨 
            var spell = SpellsDefine.StandardStep.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}