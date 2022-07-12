using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage.GCD
{
    public class WhiteMageBaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // don't cast dosis when moving, instead going to cast other spells.
            if (!MovementManager.IsMoving) return 0;

            LogHelper.Debug("Not casting Stone, since we are moving.");
            return -10;

        }
        public async Task<SpellEntity> Run()
        {
            var spell = WhiteMageSpellHelper.GetBaseGcd();
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}
