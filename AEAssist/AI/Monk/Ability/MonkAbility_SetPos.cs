using AEAssist.Define;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_SetPosition : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return 0;
        }



        public Task<SpellEntity> Run()
        {
            //
            // var spell = SpellsDefine.Brotherhood.GetSpellEntity();
            // if (spell == null)
            //     return null;
            // var ret = await spell.DoAbility();
            // if (ret)
            //     return spell;
            MonkSpellHelper.SetPostion();
            return Task.FromResult<SpellEntity>(null);
        }
    }
}