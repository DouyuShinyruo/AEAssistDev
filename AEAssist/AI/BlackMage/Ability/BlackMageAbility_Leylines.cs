using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.Ability
{
    public class BlackMageAblity_Leylines : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // if setting is no burst
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.LeyLines.IsReady())
            {
                return -1;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.LeyLines.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}