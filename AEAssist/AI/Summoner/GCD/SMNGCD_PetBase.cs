using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.GCD
{
    public class SMNGCD_PetBase : IAIHandler
    {
        uint spell;

        static public uint GetSpell()
        {
            if (SMN_SpellHelper.CheckUseAOE() && DataBinding.Instance.UseAOE)
            {
                var aoeSpell = GetAOE();
                if (aoeSpell != 0)
                    return aoeSpell;
            }

            return GetSingleTarget();
        }

        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Summoner.PetTimer <= 0)
                return -3;
            spell = GetSpell();
            if (spell == 0)
                return -4;
            if (!spell.IsReady())
            {
                return -1;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }

        public static uint GetSingleTarget()
        {
            if (SMN_SpellHelper.Ifrit())
            {
                if (MovementManager.IsMoving)
                    return 0;
                if (SpellsDefine.RubyRite.IsUnlock())
                    return SpellsDefine.RubyRite;

                if (SpellsDefine.RubyRuinIII.IsUnlock())
                    return SpellsDefine.RubyRuinIII;

                if (SpellsDefine.RubyRuinII.IsUnlock())
                    return SpellsDefine.RubyRuinII;

                return SpellsDefine.RubyRuin;
            }

            if (SMN_SpellHelper.Garuda())
            {
                if (SpellsDefine.EmeraldRite.IsUnlock())
                    return SpellsDefine.EmeraldRite;

                if (SpellsDefine.EmeraldRuinIII.IsUnlock())
                    return SpellsDefine.EmeraldRuinIII;

                if (SpellsDefine.EmeraldRuinII.IsUnlock())
                    return SpellsDefine.EmeraldRuinII;

                return SpellsDefine.EmeraldRuin;
            }

            if (SMN_SpellHelper.Titan())
            {
                if (SpellsDefine.TopazRite.IsUnlock())
                    return SpellsDefine.TopazRite;

                if (SpellsDefine.TopazRuinIII.IsUnlock())
                    return SpellsDefine.TopazRuinIII;

                if (SpellsDefine.TopazRuinII.IsUnlock())
                    return SpellsDefine.TopazRuinII;

                return SpellsDefine.TopazRuin;
            }

            return 0;
        }



        static public uint GetAOE()
        {
            if (SMN_SpellHelper.Ifrit())
            {
                if (MovementManager.IsMoving)
                    return 0;
                if (SpellsDefine.RubyDisaster.IsUnlock())
                    return SpellsDefine.RubyCatastrophe;

                if (SpellsDefine.RubyCatastrophe.IsUnlock())
                    return SpellsDefine.RubyDisaster;

                if (SpellsDefine.RubyOutburst.IsUnlock())
                    return SpellsDefine.RubyOutburst;

            }

            if (SMN_SpellHelper.Garuda())
            {
                if (SpellsDefine.EmeraldDisaster.IsUnlock())
                    return SpellsDefine.EmeraldCatastrophe; 

                if (SpellsDefine.EmeraldCatastrophe.IsUnlock())
                    return SpellsDefine.EmeraldDisaster;

                if (SpellsDefine.EmeraldOutburst.IsUnlock())
                    return SpellsDefine.EmeraldOutburst;
            }

            if (SMN_SpellHelper.Titan())
            {
                if (SpellsDefine.TopazDisaster.IsUnlock())
                    return SpellsDefine.TopazCatastrophe; 

                if (SpellsDefine.TopazCatastrophe.IsUnlock())
                    return SpellsDefine.TopazDisaster;

                if (SpellsDefine.TopazOutburst.IsUnlock())
                    return SpellsDefine.TopazOutburst;
            }

            return 0;
        }
    }
}