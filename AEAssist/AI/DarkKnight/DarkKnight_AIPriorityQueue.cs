using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.AI.DarkKnight.Ability;
using AEAssist.AI.DarkKnight.GCD;
using ff14bot.Enums;
namespace AEAssist.AI.DarkKnight
{
    [Job(ClassJobType.DarkKnight)]
    public class DarkKnight_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            //new DarkKnightGCD_BldeofXX(),
            //new DarkKnightGCD_HolySpirit(),
            //new DarkKnightGCD_Ranged(),
            //new DarkKnightGCD_Dot(),
            new DarkKnightGCD_Base()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            //new DarkKnightAbility_IronWill(),
            //new DarkKnightAbility_FightorFlight(),
            //new DarkKnightAbility_Requiescat(),
            //new DarkKnightAbility_SpiritsWithin(),
            //new DarkKnightAbility_CircleofScorn(),
            //new DarkKnightAbility_Intervene(),
            new DarkKnightAbility_defense()
        };
        public Task<bool> UsePotion()
        {
            return Task.FromResult(false);
        }
    }
}