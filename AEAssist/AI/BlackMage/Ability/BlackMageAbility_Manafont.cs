using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.Ability
{
    public class BlackMageAblity_Manafont : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // if setting is no burst
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.ManaFont.IsReady())
            {
                return -1;
            }
            if (BlackMageHelper.IsMaxAstralStacks() &&
                Core.Me.MaxMana < 800)
            {
                return 1;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.ManaFont.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}