using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_RiddleOfWind : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.RiddleofWind.IsUnlock())
            {
                return -10;
            }

            if (!SpellsDefine.RiddleofWind.IsReady())
            {
                return -1;
            }

            return 0;
        }



        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.RiddleofWind.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}