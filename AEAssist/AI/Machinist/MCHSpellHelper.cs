using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist
{
    public static class MCHSpellHelper
    {
        public static SpellEntity GetSplitShot()
        {
            if (SpellsDefine.HeatedSplitShot.IsUnlock())
                return SpellsDefine.HeatedSplitShot.GetSpellEntity();
            return SpellsDefine.SplitShot.GetSpellEntity();
        }

        public static SpellEntity GetSlugShot()
        {
            if (SpellsDefine.HeatedSlugShot.IsUnlock())
                return SpellsDefine.HeatedSlugShot.GetSpellEntity();
            return SpellsDefine.SlugShot.GetSpellEntity();
        }

        public static SpellEntity GetCleanShot()
        {
            if (SpellsDefine.HeatedCleanShot.IsUnlock())
                return SpellsDefine.HeatedCleanShot.GetSpellEntity();
            return SpellsDefine.CleanShot.GetSpellEntity();
        }

        public static SpellEntity GetSpreadShot()
        {
            if (SpellsDefine.Scattergun.IsUnlock())
                return SpellsDefine.Scattergun.GetSpellEntity();
            return SpellsDefine.SpreadShot.GetSpellEntity();
        }

        public static async Task<SpellEntity> UseBaseComboGCD(GameObject target)
        {
            if (TargetHelper.CheckNeedUseAOE(target, 12, 12))
            {
                var aoeGCD = GetSpreadShot();
                if (await aoeGCD.DoGCD())
                {
                    return aoeGCD;
                }
            }


            var lastSpellId = SpellHelper.GetLastComboSpell();

            if (lastSpellId == SpellsDefine.SplitShot || lastSpellId == SpellsDefine.HeatedSplitShot)
            {
                var slugShot = GetSlugShot();
                if (await slugShot.DoGCD())
                {
                    return slugShot;
                }
            }

            if (lastSpellId == SpellsDefine.SlugShot || lastSpellId == SpellsDefine.HeatedSlugShot)
            {
                var cleanShot = GetCleanShot();
                if (await cleanShot.DoGCD())
                {
                    return cleanShot;
                }
            }

            var splitShot = GetSplitShot();
            if (await splitShot.DoGCD())
            {
                return splitShot;
            }

            return null;
        }

        public static uint GetAirAnchor()
        {
            if (SpellsDefine.AirAnchor.IsUnlock())
                return SpellsDefine.AirAnchor;
            if (SpellsDefine.HotShot.IsUnlock())
                return SpellsDefine.HotShot;
            return 0;
        }

        public static SpellEntity GetReassembleGCD()
        {
            SpellEntity spell = null;
            if (SpellsDefine.ChainSaw.IsReady())
                spell = SpellsDefine.ChainSaw.GetSpellEntity();
            else if (SpellsDefine.AirAnchor.IsReady())
                spell = SpellsDefine.AirAnchor.GetSpellEntity();
            else if (SpellsDefine.Drill.IsReady())
                spell = SpellsDefine.Drill.GetSpellEntity();

            return spell;
        }

        public static SpellEntity GetAutomatonQueen()
        {
            if (SpellsDefine.AutomationQueen.IsUnlock())
                return SpellsDefine.AutomationQueen.GetSpellEntity();
            return SpellsDefine.RookAutoturret.GetSpellEntity();
        }

        public static SpellEntity GetQueenOverdrive()
        {
            if (SpellsDefine.QueenOverdrive.IsUnlock())
                return SpellsDefine.QueenOverdrive.GetSpellEntity();
            return SpellsDefine.RookOverdrive.GetSpellEntity();
        }

        public static bool CheckReassmableGCD(int timeleft)
        {
            if (SpellsDefine.ChainSaw.IsUnlock() &&
                SpellsDefine.ChainSaw.GetSpellEntity().Cooldown.TotalMilliseconds < timeleft)
                return true;
            if (SpellsDefine.Drill.IsUnlock() &&
                SpellsDefine.Drill.GetSpellEntity().Cooldown.TotalMilliseconds < timeleft)
                return true;
            if (SpellsDefine.AirAnchor.IsUnlock() &&
                SpellsDefine.AirAnchor.GetSpellEntity().Cooldown.TotalMilliseconds < timeleft)
                return true;
            return false;
        }

        public static uint GetDrillIfWithAOE()
        {
            if (SpellsDefine.Bioblaster.IsUnlock() && TargetHelper.CheckNeedUseAOE(Core.Me.CurrentTarget, 12, 12)) return SpellsDefine.Bioblaster;

            return SpellsDefine.Drill;
        }

        public static SpellEntity GetUnderHyperChargeGCD()
        {
            if (SpellsDefine.AutoCrossbow.IsUnlock() && TargetHelper.CheckNeedUseAOE(12, 12))
                return SpellsDefine.AutoCrossbow.GetSpellEntity();

            return SpellsDefine.HeatBlast.GetSpellEntity();
        }
    }
}