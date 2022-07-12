using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Scholar
{
    public static class Scholar_SpellHelper
    {

        public static bool Debugging { get; set; } = true;

        public static bool FightorFlightCooldownSoon()
        {
            if (!SpellsDefine.FightorFlight.IsUnlock())
                return false;

            if (Core.Me.HasAura(AurasDefine.FightOrFight))
                return false;

            //If we have requiescat, we are not using fightorflight even it is ready
            if (Core.Me.HasAura(AurasDefine.Requiescat))
                return false;
            
            if (SpellsDefine.FightorFlight.CoolDownInGCDs(3))
                return true;

            return false;
        }
        public static SpellEntity GetAero()
        {
            if (!SpellsDefine.Aero.IsUnlock())
            {
                LogHelper.Debug("Aero not unlocked. skipping.");
                return null;
            }

            if (!SpellsDefine.Aero2.IsUnlock())
            {
                LogHelper.Debug("Aero2 not unlocked trying to use Aero instead.");
                if (!ActionManager.HasSpell(SpellsDefine.Aero))
                {
                    LogHelper.Debug("Aero not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Aero found trying to use..");
                return SpellsDefine.Aero.GetSpellEntity();
            }

            if (!SpellsDefine.Dia.IsUnlock())
            {
                LogHelper.Debug("Dia not unlocked trying to use Aero2 instead.");
                if (!ActionManager.HasSpell(SpellsDefine.Aero2))
                {
                    LogHelper.Debug("Aero2 not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Aero2 found trying to use..");
                return SpellsDefine.Aero2.GetSpellEntity();
            }
            if (!ActionManager.HasSpell(SpellsDefine.Dia))
            {
                LogHelper.Debug("Dia not found. skipping.");
                return null;
            }

            LogHelper.Debug("Dia found trying to use..");
            return SpellsDefine.Dia.GetSpellEntity();
        }
        private static uint GetCombustAura()
        {
            if (!SpellsDefine.Biolysis.IsUnlock())
            {
                return SpellsDefine.Biolysis;
            }
            if (!SpellsDefine.Bio2.IsUnlock())
            {
                return SpellsDefine.Bio2;
            }

            return AurasDefine.Bio;
        }
        public static SpellEntity GetCombust()
        {
            if (SpellsDefine.Biolysis.IsUnlock())
            {
                return SpellsDefine.Biolysis.GetSpellEntity();
            }
            if (SpellsDefine.Bio2.IsUnlock())
            {
                return SpellsDefine.Bio2.GetSpellEntity();
            }

            return SpellsDefine.Bio.GetSpellEntity();
        }
        public static bool IsTargetNeedCombust(Character target, int timeLeft)
        {
            var id = GetCombustAura();
            LogHelper.Debug("Checking if target has Combust: " + target.EnglishName);
            return id == 0 || target.HasMyAuraWithTimeleft((uint)id);
        }
        public static bool IsTargetHasAuraCombust(Character target)
        {
            var id = GetCombustAura();
            LogHelper.Debug("Checking if target has Combust: " + target.EnglishName);
            return id == 0 || target.HasMyAuraWithTimeleft((uint)id);
        }
        public static bool OutOfMeleeRange()
        {
            return !Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget, 3);
        }

        //AOE 这个距离判定有点迷
        public static bool OutOfAOERange()
        {
            return Core.Me.Distance(Core.Me.CurrentTarget) > 5 && OutOfMeleeRange();
        }

        public static bool CheckUseAOE(int count = 3)
        {
            if (!DataBinding.Instance.UseAOE)
                return false;

            if (TargetHelper.CheckNeedUseAOE(0, 5, count))
                return true;
            return false;
        }

        public static bool NeedRenewDot(uint DotSpell)
        {
            //if (CheckUseAOE())
            //    return false;
            
            var target = Core.Me.CurrentTarget as Character;
            //LogHelper.Info($"Target's dot expires in {target.GetAuraById(DotSpell).TimeLeft} ms");
            if (target == null)
                return false;
            if (target.HasMyAura(DotSpell))
                if (!target.HasMyAuraWithTimeleft(DotSpell, 3000))//id，剩余时间
                {
                    LogHelper.Info($"Target's dot expires in {target.GetAuraById(DotSpell).TimeLeft} ms, renewing dot.");
                    return true;
                }
                else return false;          
            
            return true;
        }

        public static int GCDNeededforCombo()
        {
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.FastBlade:
                    return 2;
                case SpellsDefine.RiotBlade:
                    return 1;
                default:
                    return 3;
            }
        }


    }
}