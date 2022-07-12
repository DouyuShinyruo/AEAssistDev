using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_RiddleOfFire : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.RiddleofFire.IsUnlock())
            {
                return -10;
            }

            if (!SpellsDefine.RiddleofFire.IsReady())
            {
                return -1;
            }

            if (!AIRoot.Instance.Is2ndAbilityTime(0.6f))
                return -11;

            return 0;
        }



        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.RiddleofFire.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}