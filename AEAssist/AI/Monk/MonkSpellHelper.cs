using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk
{
    public class MonkSpellHelper
    {
        public static void SetPostion()
        {
            if (!Core.OverlayManager.IsActive)
            {
                return;
            }

            if (Core.Me.CurrentJob != ClassJobType.Monk)
            {
                return;
            }

            if (!Core.Me.InCombat)
            {
                return;
            }

            if (!Core.Me.HasTarget)
            {
                return;
            }

            if (Core.Me.HasAura(AurasDefine.RiddleOfFire) &&
                !Core.Me.HasMyAuraWithTimeleft(AurasDefine.RiddleOfFire, 3000))
            {
                AIRoot.GetBattleData<MonkBattleData>().RoFBH2 = false;
                AIRoot.GetBattleData<MonkBattleData>().CurrentBurst = MonkBurst.None;
            }

            if (!Core.Me.HasAura(AurasDefine.RiddleOfFire) && SpellsDefine.RiddleofFire.CoolDownInGCDs(7))
            {
                IsEvenBurst();
            }

            if (!Core.Me.HasTarget)
            {
                MeleePosition.Intance.SetPositionToNone();
                MeleePosition.Intance.ShowMsg();
                return;
            }

            if (AIRoot.GetBattleData<MonkBattleData>().DoingOpener)
            {
                MeleePosition.Intance.SetPositionToBack();
                MeleePosition.Intance.ShowMsg();
                return;
            }

            var target = Core.Me.CurrentTarget as Character;

            var priority = MeleePosition.Priority.Low;

            if (InRaptorForm())
            {
                priority = MeleePosition.Priority.Medium;
            }
            else if (InCoeurlForm())
            {
                priority = MeleePosition.Priority.High;
            }

            if (SpellsDefine.PerfectBalance.RecentlyUsed() || Core.Me.HasMyAura(AurasDefine.PerfectBalance))
            {
                var currentNadi = AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo;
                if (currentNadi == MonkNadiCombo.Lunar)
                {
                    MeleePosition.Intance.SetPositionToNone(priority);
                    MeleePosition.Intance.ShowMsg();
                    return;
                }

                if (currentNadi == MonkNadiCombo.Solar)
                {
                    if (AIRoot.GetBattleData<MonkBattleData>().RoFBH2)
                    {
                        MeleePosition.Intance.SetPositionToSide(priority);
                        MeleePosition.Intance.ShowMsg();
                        return;
                    }
                    else
                    {
                        MeleePosition.Intance.SetPositionToBack(priority);
                        MeleePosition.Intance.ShowMsg();
                        return;
                    }
                }
            }

            if (UsingDot())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 5))
                {
                    MeleePosition.Intance.SetPositionToNone(priority);
                    MeleePosition.Intance.ShowMsg();
                    return;
                }

                if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 6000) == false)
                {
                    MeleePosition.Intance.SetPositionToBack(priority);
                    MeleePosition.Intance.ShowMsg();
                    return;
                }
            }

            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
            {
                MeleePosition.Intance.SetPositionToNone(priority);
                MeleePosition.Intance.ShowMsg();
                return;
            }

            MeleePosition.Intance.SetPositionToSide(priority);
            MeleePosition.Intance.ShowMsg();
            return;
        }

        public static void IsEvenBurst()
        {
            // if we have two stacks of pb comes up during rof, then it is even
            // rof -> 20s -> 11 gcd
            // rof CD + 7 gcd -> how many stacks of pb
            // if 2 > even burst
            // if 1 > odd burst
            if (AIRoot.GetBattleData<MonkBattleData>().CurrentBurst != MonkBurst.None)
            {
                return;
            }
            int gcdsUntilRof = SpellsDefine.RiddleofFire.AbilityComesInNextXGCDsWindow();
            int gcdsUntilDeadline = gcdsUntilRof + 7;
            var baseGCDTime = RotationManager.Instance.GetBaseGCDSpell().AdjustedCooldown.TotalMilliseconds;
            var delta = TimeHelper.Now() - AIRoot.GetBattleData<BattleData>().lastCastTime;
            var timeUntilNextGCD = baseGCDTime - delta;
            var msUntilDeadline = timeUntilNextGCD + baseGCDTime * gcdsUntilDeadline;
            if (SpellsDefine.PerfectBalance.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds < msUntilDeadline)
            {
                if (AIRoot.GetBattleData<MonkBattleData>().CurrentBurst == MonkBurst.None)
                {
                    AIRoot.GetBattleData<MonkBattleData>().CurrentBurst = MonkBurst.Even;
                }
            }
            else
            {
                if (AIRoot.GetBattleData<MonkBattleData>().CurrentBurst == MonkBurst.None)
                {
                    AIRoot.GetBattleData<MonkBattleData>().CurrentBurst = MonkBurst.Odd;
                }
            }
            return;
        }
        
        public static bool InCoeurlForm()
        {
            if (Core.Me.HasMyAura(AurasDefine.CoeurlForm))
            {
                return true;
            }

            if (ActionManager.LastSpellId == SpellsDefine.TrueStrike ||
                ActionManager.LastSpellId == SpellsDefine.TwinSnakes ||
                ActionManager.LastSpellId == SpellsDefine.FourPointFury)
            {
                return true;
            }

            return false;
        }

        public static bool InOpoOpoForm()
        {
            if (Core.Me.HasMyAura(AurasDefine.OpoOpoForm))
            {
                return true;
            }

            if (ActionManager.LastSpellId == SpellsDefine.Demolish ||
                ActionManager.LastSpellId == SpellsDefine.SnapPunch ||
                ActionManager.LastSpellId == SpellsDefine.Rockbreaker)
            {
                return true;
            }

            return false;
        }

        public static bool InRaptorForm()
        {
            if (Core.Me.HasMyAura(AurasDefine.RaptorForm))
            {
                return true;
            }

            if (ActionManager.LastSpellId == SpellsDefine.Bootshine ||
                ActionManager.LastSpellId == SpellsDefine.DragonKick ||
                ActionManager.LastSpellId == SpellsDefine.ArmOfTheDestroyer)
            {
                return true;
            }

            return false;
        }


        public static async Task<SpellEntity> DoOpoOpoGCDS(Character target, bool pb = false)
        {
            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
            {
                if (SpellsDefine.ShadowOfTheDestroyer.IsUnlock())
                {
                    if (await SpellsDefine.ShadowOfTheDestroyer.DoGCD())
                    {
                        return SpellsDefine.ShadowOfTheDestroyer.GetSpellEntity();
                    }
                }

                if (!pb)
                {
                    if (SpellsDefine.ArmOfTheDestroyer.IsUnlock())
                    {
                        //Arm of the Destroyer 破坏神冲
                        if (await SpellsDefine.ArmOfTheDestroyer.DoGCD())
                        {
                            return SpellsDefine.ArmOfTheDestroyer.GetSpellEntity();
                        }
                    }
                }

            }

            if (Core.Me.HasMyAura(AurasDefine.LeadenFist) || !SpellsDefine.DragonKick.IsUnlock())
            {
                //Bootshine 连击
                if (await SpellsDefine.Bootshine.DoGCD())
                {
                    return SpellsDefine.Bootshine.GetSpellEntity();
                }
            }

            //Dragon Kick 双龙脚
            if (await SpellsDefine.DragonKick.DoGCD())
            {
                return SpellsDefine.DragonKick.GetSpellEntity();
            }

            return null;
        }

        private static async Task<SpellEntity> DoRaptorGCDS(Character target)
        {
            if (InRaptorForm())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                {
                    if (SpellsDefine.FourPointFury.IsUnlock())
                    {
                        //Four-point Fury 四面脚
                        if (await SpellsDefine.FourPointFury.DoGCD())
                        {
                            return SpellsDefine.FourPointFury.GetSpellEntity();
                        }
                    }
                }

                //Pre Rof
                if (!Core.Me.HasMyAura(AurasDefine.RiddleOfFire) &&
                    SpellsDefine.RiddleofFire.AbilityCoolDownInNextXGCDsWindow(5))
                {
                    //Even Window
                    if (SpellsDefine.Brotherhood.AbilityCoolDownInNextXGCDsWindow(10))
                    {
                        if (SpellsDefine.RiddleofFire.AbilityCoolDownInNextXGCDsWindow(3))
                        {
                            if (!target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 7000))
                            {
                                if (await SpellsDefine.TwinSnakes.DoGCD())
                                {
                                    return SpellsDefine.TwinSnakes.GetSpellEntity();
                                }
                            }
                        }
                    }
                    //Odd window
                    else
                    {
                        if (SpellsDefine.RiddleofFire.AbilityCoolDownInNextXGCDsWindow(5))
                        {
                            if (!target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 7000))
                            {
                                if (await SpellsDefine.TwinSnakes.DoGCD())
                                {
                                    return SpellsDefine.TwinSnakes.GetSpellEntity();
                                }
                            }
                        }
                    }
                }

                if (Core.Me.HasMyAura(AurasDefine.RiddleOfFire))
                {
                    if (!target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 7000))
                    {
                        if (await SpellsDefine.TwinSnakes.DoGCD())
                        {
                            return SpellsDefine.TwinSnakes.GetSpellEntity();
                        }
                    }
                }

                if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 8000))
                {
                    //True Strike 正拳
                    if (await SpellsDefine.TrueStrike.DoGCD())
                    {
                        return SpellsDefine.TrueStrike.GetSpellEntity();
                    }
                }

                //Twin Snakes 双掌打
                if (await SpellsDefine.TwinSnakes.DoGCD())
                {
                    return SpellsDefine.TwinSnakes.GetSpellEntity();
                }
            }

            return null;
        }

        public static SpellEntity GetCoeurlGCDS(Character target)
        {
            if (InCoeurlForm())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                {
                    if (SpellsDefine.Rockbreaker.IsUnlock())
                    {
                        //Rockbreaker 地烈劲
                        return SpellsDefine.Rockbreaker.GetSpellEntity();
                    }
                }

                if (UsingDot() == false)
                {
                    //Snap Punch 崩拳
                    return SpellsDefine.SnapPunch.GetSpellEntity();
                }

                if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 5000))
                {
                    //Snap Punch 崩拳
                    return SpellsDefine.SnapPunch.GetSpellEntity();
                }

                //Demolish 破碎拳
                return SpellsDefine.Demolish.GetSpellEntity();
            }

            return null;
        }

        private static async Task<SpellEntity> DoCoeurlGCDS(Character target)
        {
            if (InCoeurlForm())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                {
                    if (SpellsDefine.Rockbreaker.IsUnlock())
                    {
                        //Rockbreaker 地烈劲
                        if (await SpellsDefine.Rockbreaker.DoGCD())
                        {
                            return SpellsDefine.Rockbreaker.GetSpellEntity();
                        }
                    }
                }

                if (UsingDot() == false)
                {
                    //Snap Punch 崩拳
                    if (await SpellsDefine.SnapPunch.DoGCD())
                    {
                        return SpellsDefine.SnapPunch.GetSpellEntity();
                    }
                }

                if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 5000))
                {
                    //Snap Punch 崩拳
                    if (await SpellsDefine.SnapPunch.DoGCD())
                    {
                        return SpellsDefine.SnapPunch.GetSpellEntity();
                    }
                }

                //Demolish 破碎拳
                if (await SpellsDefine.Demolish.DoGCD())
                {
                    return SpellsDefine.Demolish.GetSpellEntity();
                }
            }

            return null;
        }


        public static async Task<SpellEntity> BaseGCDCombo(Character target)
        {
            if (InCoeurlForm())
            {
                return await DoCoeurlGCDS(target);
            }

            if (InRaptorForm())
            {
                return await DoRaptorGCDS(target);
            }

            return await DoOpoOpoGCDS(target);
        }

        public static bool UsingDot()
        {
            if (!AEAssist.DataBinding.Instance.UseDot)
            {
                return false;
            }

            if (!SpellsDefine.Demolish.IsUnlock())
            {
                return false;
            }

            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 5))
            {
                return false;
            }

            var target = Core.Me.CurrentTarget as Character;

            if (DotBlacklistHelper.IsBlackList(target))
                return false;

            if (TTKHelper.IsTargetTTK(target, 11, false))
            {
                return false;
            }

            return true;
        }

        private static async Task<SpellEntity> UseLunarNadiCombo(Character target)
        {
            return await DoOpoOpoGCDS(target);
        }

        private static async Task<SpellEntity> UseSolarNadiCombo(Character target)
        {
            //combo1
            if (!ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.Raptor))
            {
                // if we have ROF, buff, go first
                if (Core.Me.HasMyAura(AurasDefine.RiddleOfFire) || SpellsDefine.RiddleofFire.RecentlyUsed())
                {
                    if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 2000))
                    {
                        return await DoOpoOpoGCDS(target, true);
                    }
                }
                // if we not have ROF, other two is done, we have to use it
                else if (ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.Coeurl) &&
                         ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.OpoOpo))
                {
                    return await DoOpoOpoGCDS(target, true);
                }
            }

            //combo2
            if (!ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.Coeurl))
            {
                // if we need it maintain the buff
                if (!Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 3000))
                {
                    LogHelper.Info("PerfectBalance - combo 2 - do it to refresh buff");
                    return await PerfectBalanceCoeurl(target);
                }

                // if we not having RoF, it goes first, because high dmg goes to later
                if (!Core.Me.HasMyAura(AurasDefine.RiddleOfFire) && !SpellsDefine.RiddleofFire.RecentlyUsed())
                {
                    LogHelper.Info(
                        "PerfectBalance - combo 2 - do it because we don't have ROF, low dmg combo 2 go first");
                    return await PerfectBalanceCoeurl(target);
                }

                // if we have other two, we have to use this
                if (ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.Raptor) &&
                    ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.OpoOpo))
                {
                    LogHelper.Info("PerfectBalance - combo 2 - do it because other two is done");
                    return await PerfectBalanceCoeurl(target);
                }
            }

            //combo 3
            if (!ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.OpoOpo))
            {
                if (Core.Me.HasMyAura(AurasDefine.RiddleOfFire) || SpellsDefine.RiddleofFire.RecentlyUsed())
                {
                    if (UsingDot() && !target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 5000))
                    {
                        if (await SpellsDefine.Demolish.DoGCD())
                        {
                            LogHelper.Info("PerfectBalance - combo 3 - do it in rof to refresh dot");
                            return SpellsDefine.Demolish.GetSpellEntity();
                        }
                    }
                    else
                    {
                        if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                        {
                            if (await SpellsDefine.Rockbreaker.DoGCD())
                            {
                                return SpellsDefine.Rockbreaker.GetSpellEntity();
                            }
                        }

                        if (await SpellsDefine.SnapPunch.DoGCD())
                        {
                            LogHelper.Info("PerfectBalance - combo 3 - do it because we don't need to refresh dot");
                            return SpellsDefine.SnapPunch.GetSpellEntity();
                        }
                    }
                }

                if (ActionResourceManager.Monk.MastersGauge.Contains(ActionResourceManager.Monk.Chakra.Coeurl) &&
                    !Core.Me.HasMyAura(AurasDefine.RiddleOfFire))
                {
                    if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                    {
                        if (await SpellsDefine.Rockbreaker.DoGCD())
                        {
                            return SpellsDefine.Rockbreaker.GetSpellEntity();
                        }
                    }

                    if (await SpellsDefine.SnapPunch.DoGCD())
                    {
                        LogHelper.Info(
                            "PerfectBalance - combo 3 - do it because we don't have RoF, combo2 is done, push combo 1 to later,");
                        return SpellsDefine.SnapPunch.GetSpellEntity();
                    }
                }
            }


            return null;
        }

        private static Task<SpellEntity> PerfectBalanceOpoOpo(Character target)
        {
            return Task.FromResult<SpellEntity>(null);
        }

        private static async Task<SpellEntity> PerfectBalanceCoeurl(Character target)
        {
            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
            {
                if (await SpellsDefine.FourPointFury.DoGCD())
                {
                    return SpellsDefine.FourPointFury.GetSpellEntity();
                }
            }

            if (await SpellsDefine.TwinSnakes.DoGCD())
            {
                LogHelper.Info("PerfectBalance - combo 2 - do it to refresh buff");
                return SpellsDefine.TwinSnakes.GetSpellEntity();
            }

            return null;
        }

        private static async Task<SpellEntity> PerfectBalanceRaptor(Character target)
        {
            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
            {
                if (await SpellsDefine.ShadowOfTheDestroyer.DoGCD())
                {
                    return SpellsDefine.ShadowOfTheDestroyer.GetSpellEntity();
                }
            }

            if (Core.Me.HasMyAura(AurasDefine.LeadenFist))
            {
                //Bootshine 连击
                if (await SpellsDefine.Bootshine.DoGCD())
                {
                    return SpellsDefine.Bootshine.GetSpellEntity();
                }
            }

            //Dragon Kick 双龙脚
            if (await SpellsDefine.DragonKick.DoGCD())
            {
                return SpellsDefine.DragonKick.GetSpellEntity();
            }

            return null;
        }

        public static async Task<SpellEntity> PerfectBalanceGCDCombo(Character target)
        {
            if (!SpellsDefine.ElixirField.IsUnlock())
            {
                AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Lunar;
            }
            else
            {
                //有阳打阴 两个都有也打阴
                if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Both ||
                    ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Solar)
                {
                    AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Lunar;
                }
                //有阴打阳
                else if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Lunar)
                {
                    AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Solar;
                }
                //什么都没有 
                //todo 如果时间都够 怎么接下来打阳
                else if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.None &&
                         AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo == MonkNadiCombo.None)
                {
                    //如果双buff时间都够 打阴
                    if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 7000) &&
                        target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 9000))
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Lunar;
                    }
                    //如果时间不够打阳
                    else
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Solar;
                    }
                }
            }

            if (AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo == MonkNadiCombo.Solar)
            {
                return await UseSolarNadiCombo(target);
            }

            return await UseLunarNadiCombo(target);
        }

        public static bool LastSpellWasGCD()
        {
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            SpellEntity[] GCDs =
            {
                SpellsDefine.Bootshine.GetSpellEntity(),
                SpellsDefine.DragonKick.GetSpellEntity(),
                SpellsDefine.ArmOfTheDestroyer.GetSpellEntity(),
                SpellsDefine.ShadowOfTheDestroyer.GetSpellEntity(),
                SpellsDefine.TwinSnakes.GetSpellEntity(),
                SpellsDefine.TrueStrike.GetSpellEntity(),
                SpellsDefine.FourPointFury.GetSpellEntity(),
                SpellsDefine.Rockbreaker.GetSpellEntity(),
                SpellsDefine.Demolish.GetSpellEntity(),
                SpellsDefine.SnapPunch.GetSpellEntity(),
            };
            if (GCDs.Contains(bdls))
            {
                return true;
            }

            return false;
        }

        public static bool LastSpellWasNadi()
        {
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            SpellEntity[] GCDs =
            {
                SpellsDefine.ElixirField.GetSpellEntity(),
                SpellsDefine.FlintStrike.GetSpellEntity(),
                SpellsDefine.CelestialRevolution.GetSpellEntity(),
                SpellsDefine.TornadoKick.GetSpellEntity(),
                SpellsDefine.RisingPhoenix.GetSpellEntity(),
                SpellsDefine.PhantomRush.GetSpellEntity()
            };
            if (GCDs.Contains(bdls))
            {
                return true;
            }

            return false;
        }

        public void test()
        {
            var distance2D = Core.Me.CurrentTarget.Distance2D();
            var ObjectRaduis = Core.Me.CurrentTarget.CombatReach;
        }
    }
}