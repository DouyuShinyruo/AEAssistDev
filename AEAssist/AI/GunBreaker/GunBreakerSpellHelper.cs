using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.GunBreaker
{
    public class GunBreakerSpellHelper
    {
        public static SpellEntity GetBaseSpell()
        {
            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 3))
                return UseAoe();

            return SingleTarget();
        }

        public static SpellEntity UseAoe()
        {
            if (ActionResourceManager.Gunbreaker.Cartridge > 0)
            {
                if (Core.Me.ClassLevel < 90)
                    return SpellsDefine.FatedCircle.GetSpellEntity();
                //
                else if (SpellsDefine.DoubleDown.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds > 8000)
                    return SpellsDefine.FatedCircle.GetSpellEntity();
            }
            if (ActionManager.LastSpell == SpellsDefine.DemonSlice.GetSpellEntity().SpellData)
                return SpellsDefine.DemonSlaughter.GetSpellEntity();

            return SpellsDefine.DemonSlice.GetSpellEntity();
        }
        public static SpellEntity SingleTarget()
        {
            if (ActionManager.LastSpell.Id == SpellsDefine.KeenEdge)
                return SpellsDefine.BrutalShell.GetSpellEntity();

            if (ActionManager.LastSpell.Id == SpellsDefine.BrutalShell)
                return SpellsDefine.SolidBarrel.GetSpellEntity();

            return SpellsDefine.KeenEdge.GetSpellEntity();
        }
        public static SpellEntity SecondaryCombo()
        {
            if (ActionResourceManager.Gunbreaker.SecondaryComboStage == 1)
                return SpellsDefine.SavageClaw.GetSpellEntity();

            if (ActionResourceManager.Gunbreaker.SecondaryComboStage == 2)
                return SpellsDefine.WickedTalon.GetSpellEntity();

            return SpellsDefine.GnashingFang.GetSpellEntity();
        }
        public static SpellEntity GetContinuation()
        {
            if (Core.Me.HasAura(AurasDefine.ReadytoRip))
                return SpellsDefine.JugularRip.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.ReadytoTear))
                return SpellsDefine.AbdomenTear.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.ReadytoGouge))
                return SpellsDefine.EyeGouge.GetSpellEntity();

            if (Core.Me.HasAura(AurasDefine.ReadytoBlast))
                return SpellsDefine.Hypervelocity.GetSpellEntity();

            return null;
        }
    }
}

