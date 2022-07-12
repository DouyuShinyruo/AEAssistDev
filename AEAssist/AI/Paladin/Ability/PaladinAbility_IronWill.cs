using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Paladin.Ability
{
    public class PaladinAbility_IronWill : IAIHandler
    {
        uint spell = SpellsDefine.IronWill;

        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;

            if (DataBinding.Instance.PaladinSettings.IronWill && !Core.Me.HasMyAura(AurasDefine.IronWill))
                return 1;
            if (!DataBinding.Instance.PaladinSettings.IronWill && Core.Me.HasMyAura(AurasDefine.IronWill))
                return 0;
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}