using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Paladin.Ability
{
    public class PaladinAbility_Intervene : IAIHandler
    {
        uint spell = SpellsDefine.Intervene;

        public int Check(SpellEntity lastSpell)
        {

            if (!spell.IsReady())
                return -1;
            if (!DataBinding.Instance.PaladinSettings.Intervene)
                return -2;
            if (MovementManager.IsMoving)
                return -3;

            if (Paladin_SpellHelper.OutOfMeleeRange())
                //return 2;
                return -5;

            if (SpellsDefine.FightorFlight.IsReady() || SpellsDefine.FightorFlight.CoolDownInGCDs(3))
                return -2;

            if (spell.GetSpellEntity().SpellData.Charges > 1.9)
                return 1;
            if (AIRoot.Instance.CloseBurst)
                return -4;
            if (spell.GetSpellEntity().SpellData.Charges < 1)
                return -6;
            
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}