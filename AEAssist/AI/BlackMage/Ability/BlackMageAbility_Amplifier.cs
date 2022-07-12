using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.Ability
{
    public class BlackMageAblity_Amplifier : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // if setting is no burst
            if (AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.Amplifier.IsReady())
            {
                return -1;
            }

            if (ActionResourceManager.BlackMage.PolyglotCount < 2)
            {
                return 1;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Amplifier.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}