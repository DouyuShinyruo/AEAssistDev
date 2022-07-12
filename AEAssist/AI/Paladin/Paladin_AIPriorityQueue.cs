using AEAssist.AI.Paladin.Ability;
using AEAssist.AI.Paladin.GCD;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AEAssist.AI.Paladin
{
    [Job(ClassJobType.Paladin)]
    public class Paladin_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new PaladinGCD_BldeofXX(),
            new PaladinGCD_HolySpirit(),
            new PaladinGCD_Ranged(),
            new PaladinGCD_Dot(),
            new PaladinGCD_Base()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new PaladinAbility_IronWill(),
            new PaladinAbility_FightorFlight(),
            new PaladinAbility_Requiescat(),
            new PaladinAbility_SpiritsWithin(),
            new PaladinAbility_CircleofScorn(),
            new PaladinAbility_Intervene(),
            new PaladinAbility_Sheltron()
        };
        public Task<bool> UsePotion()
        {
            return Task.FromResult(false);
        }
    }
}