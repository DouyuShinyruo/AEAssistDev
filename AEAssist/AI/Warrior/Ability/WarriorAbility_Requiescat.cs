using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Warrior.Ability
{
    public class WarriorAbility_Requiescat : IAIHandler
    {
        uint spell = SpellsDefine.Requiescat;

        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;

            if (AIRoot.Instance.CloseBurst)
                return -2;

            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.FightOrFight, 30000 - 5 * (int)AIRoot.Instance.GetGCDDuration()))
                return -3;

            if (Warrior_SpellHelper.OutOfMeleeRange())
                return -4;

            if (SpellsDefine.FightorFlight.IsReady())
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