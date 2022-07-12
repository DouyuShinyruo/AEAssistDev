using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.GCD
{
    public class SMNGCD_PetIfritCrimson : IAIHandler
    {
        static
        uint spell;
        uint GetSpell()
        {
            if (Core.Me.HasAura(AurasDefine.IfritsFavor))
                return SpellsDefine.CrimsonCyclone;
            if (ActionManager.LastSpellId == SpellsDefine.CrimsonCyclone)
                return SpellsDefine.CrimsonStrike;
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (spell == 0)
                return -3;
            if (!SMN_SpellHelper.Ifrit())
                return -4;
            if (!spell.IsReady())
                return -1;
            if (spell == SpellsDefine.CrimsonStrike && !Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget, 3))
                return -5;
            if (!DataBinding.Instance.SMNSettings.Crimson)
            {
                return -6;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}