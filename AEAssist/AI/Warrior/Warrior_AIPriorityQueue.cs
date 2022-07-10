using AEAssist.AI.Warrior.Ability;
using AEAssist.AI.Warrior.GCD;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AEAssist.AI.Warrior
{
    [Job(ClassJobType.Warrior)]
    public class Warrior_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new WarriorGCD_BldeofXX(),
            new WarriorGCD_HolySpirit(),
            new WarriorGCD_Ranged(),
            new WarriorGCD_Dot(),
            new WarriorGCD_Base()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new WarriorAbility_IronWill(),
            new WarriorAbility_FightorFlight(),
            new WarriorAbility_Requiescat(),
            new WarriorAbility_SpiritsWithin(),
            new WarriorAbility_CircleofScorn(),
            new WarriorAbility_Intervene(),
            new WarriorAbility_Sheltron()
        };
        public Task<bool> UsePotion()
        {
            return Task.FromResult(false);
        }
    }
}