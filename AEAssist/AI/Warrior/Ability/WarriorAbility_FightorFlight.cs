using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Warrior.Ability
{
    public class WarriorAbility_FightorFlight : IAIHandler
    {
        uint spell = SpellsDefine.FightorFlight;


        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;

            if (AIRoot.Instance.CloseBurst)
                return -2;


            if (!AIRoot.Instance.Is2ndAbilityTime())
                return -3;

            if (ActionManager.ComboTimeLeft <= 0)
                return -4;
            if (DataBinding.Instance.WarriorSettings.FightorFlightTiming == 1 && ActionManager.LastSpellId != SpellsDefine.FastBlade)
                return -4;
            if (DataBinding.Instance.WarriorSettings.FightorFlightTiming == 2 && ActionManager.LastSpellId != SpellsDefine.RiotBlade)
                return -4;

            if (Core.Me.HasAura(AurasDefine.Requiescat))
                return -5;
            if (Warrior_SpellHelper.OutOfMeleeRange())
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