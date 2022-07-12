using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.RedMage.GCD
{
    public class RedMageGCD_Moulinet : IAIHandler
    {
        uint spell;
     
        public int Check(SpellEntity lastSpell)
        {
            var R = ActionResourceManager.RedMage.WhiteMana > 20 && ActionResourceManager.RedMage.BlackMana > 20;

            if (ActionResourceManager.RedMage.WhiteMana > 80 && ActionResourceManager.RedMage.BlackMana > 80)
                spell = SpellsDefine.Moulinet;

            if (ActionManager.LastSpellId == SpellsDefine.Moulinet && R)//Moulinet-划圆斩
                spell = SpellsDefine.Moulinet;
            spell = 0; 

            if (spell==0)
                return 0;
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}