using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
namespace AEAssist.AI.Paladin.Ability
{
    public class PaladinAbility_CircleofScorn : IAIHandler
    {
        uint spell = SpellsDefine.CircleofScorn;

        public int Check(SpellEntity lastSpell)
        {

            if (!spell.IsReady())
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (Paladin_SpellHelper.FightorFlightInGCD())
                return -3;
            if (Paladin_SpellHelper.OutOfAOERange())
                return -4;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }
}