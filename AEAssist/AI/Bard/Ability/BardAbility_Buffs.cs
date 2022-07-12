using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_Buffs : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (AIRoot.Instance.CloseBurst)
                return -1;
            var tar = Core.Me.CurrentTarget as Character;
            if (TTKHelper.IsTargetTTK(tar))
                return -2;
            if (!BardSpellHelper.CheckCanUseBuffs())
                return -3;
            var buffs = BardSpellHelper.GetBuffs();
            if (buffs == null)
                return -4;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // if (!AIRoot.Instance.Is2ndAbilityTime())
            //     return null;
            var buff = BardSpellHelper.GetBuffs();
            if (buff == null)
                return null;
            var ret = await buff.DoAbility();
            if (ret)
            {
                return buff;
            }

            return null;
        }
    }
}