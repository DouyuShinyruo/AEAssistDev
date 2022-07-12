using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_Hagakure : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Hagakure.IsReady()) return -1;
            if (SamuraiSpellHelper.SenCounts() < 1) return -2;
            if (ActionResourceManager.Samurai.Kenki == 100) return -1;
            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                if (ActionResourceManager.Samurai.Kenki > 70) return -3;
            }
            else if (SamuraiSpellHelper.SenCounts() == 2)
            {
                if (ActionResourceManager.Samurai.Kenki > 80) return -3;
            }
            if (ActionResourceManager.Samurai.Kenki > 90) return -3;


            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Hagakure.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}