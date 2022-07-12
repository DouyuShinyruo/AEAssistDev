using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_TrueNorth : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!AEAssist.DataBinding.Instance.UseTrueNorth)
                return -10;

            if (!SpellsDefine.TrueNorth.IsReady())
                return -1;

            if (Core.Me.HasAura(AurasDefine.TrueNorth)
                || SpellsDefine.TrueNorth.RecentlyUsed())
                return -9;


            //alway use in the second half of GCD
            if (!AIRoot.Instance.Is2ndAbilityTime())
                return -11;

            var target = Core.Me.CurrentTarget as Character;

            if (MonkSpellHelper.InCoeurlForm())
            {
                var spell = MonkSpellHelper.GetCoeurlGCDS(target);
                if (spell == SpellsDefine.SnapPunch.GetSpellEntity() && !target.IsFlanking)
                {
                    return 1;
                }

                if (spell == SpellsDefine.Demolish.GetSpellEntity() && !target.IsBehind)
                {
                    return 1;
                }
            }

            if (Core.Me.HasAura(AurasDefine.PerfectBalance) &&
                AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo == MonkNadiCombo.Solar)
            {
                if (!ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.OpoOpo))
                {
                    if (Core.Me.HasMyAura(AurasDefine.RiddleOfFire) || SpellsDefine.RiddleofFire.RecentlyUsed())
                    {
                        if (MonkSpellHelper.UsingDot() && !target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 5000))
                        {
                            if (!target.IsBehind)
                            {
                                return 1;
                            }
                            // if (await SpellsDefine.Demolish.DoGCD())
                            // {
                            //     LogHelper.Info("PerfectBalance - combo 3 - do it in rof to refresh dot");
                            //     return SpellsDefine.Demolish.GetSpellEntity();
                            // }
                        }
                        else
                        {
                            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                            {
                                return -1;
                                // if (await SpellsDefine.Rockbreaker.DoGCD())
                                // {
                                //     return SpellsDefine.Rockbreaker.GetSpellEntity();
                                // }
                            }

                            if (!target.IsFlanking)
                            {
                                return 2;
                            }
                            // if (await SpellsDefine.SnapPunch.DoGCD())
                            // {
                            //     LogHelper.Info("PerfectBalance - combo 3 - do it because we don't need to refresh dot");
                            //     return SpellsDefine.SnapPunch.GetSpellEntity();
                            // }
                        }
                    }

                    if (ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.Coeurl) &&
                        !Core.Me.HasMyAura(AurasDefine.RiddleOfFire))
                    {
                        if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                        {
                            return -1;
                            // if (await SpellsDefine.Rockbreaker.DoGCD())
                            // {
                            //     return SpellsDefine.Rockbreaker.GetSpellEntity();
                            // }
                        }
                        if (!target.IsFlanking)
                        {
                            return 2;
                        }
                        // if (await SpellsDefine.SnapPunch.DoGCD())
                        // {
                        //     LogHelper.Info(
                        //         "PerfectBalance - combo 3 - do it because we don't have RoF, combo2 is done, push combo 1 to later,");
                        //     return SpellsDefine.SnapPunch.GetSpellEntity();
                        // }
                    }
                }
            }

            return -5;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.TrueNorth.GetSpellEntity();
            if (await spell.DoAbility()) return spell;

            return null;
        }
    }
}