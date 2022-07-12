using AEAssist.Define;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.GCD
{
    public class GunBreakerGCD_BaseGCDCombo : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Gunbreaker.SecondaryComboStage > 0)
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            //contain base combo
            var spell = GunBreakerSpellHelper.GetBaseSpell();
            if (await spell.DoGCD())
                return spell;
            return null;
        }
    }
}