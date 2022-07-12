using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Warrior.Ability
{
    public class WarriorAbility_Intervene : IAIHandler
    {
        uint spell = SpellsDefine.Intervene;

        public int Check(SpellEntity lastSpell)
        {

            if (!spell.IsReady())
                return -1;
            if (!SettingMgr.GetSetting<WarriorSettings>().Intervene)
                return -2;
            if (MovementManager.IsMoving)
                return -3;

            if (Warrior_SpellHelper.OutOfMeleeRange())
                //return 2;
                return -5;

            if (SpellsDefine.FightorFlight.IsReady() || SpellsDefine.FightorFlight.CoolDownInGCDs(3))
                return -2;

            if (spell.GetSpellEntity().SpellData.Charges > 1.9)
                return 1;
            if (AIRoot.Instance.CloseBurst)
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