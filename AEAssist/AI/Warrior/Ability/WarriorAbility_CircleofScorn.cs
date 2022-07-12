using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
namespace AEAssist.AI.Warrior.Ability
{
    public class WarriorAbility_CircleofScorn : IAIHandler
    {
        uint spell = SpellsDefine.CircleofScorn;

        public int Check(SpellEntity lastSpell)
        {

            if (!spell.IsReady())
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (Warrior_SpellHelper.FightorFlightCooldownSoon())
                return -3;
            if (Warrior_SpellHelper.OutOfAOERange())
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