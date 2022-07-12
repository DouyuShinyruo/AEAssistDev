using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.AI.RedMage.Ability;
using AEAssist.AI.RedMage.GCD;
using ff14bot.Enums;
namespace AEAssist.AI.RedMage
{
    [Job(ClassJobType.RedMage)]
    public class RedMage_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            //new RedMageGCD_BldeofXX(),
            //new RedMageGCD_HolySpirit(),
            //new RedMageGCD_Ranged(),
            //new RedMageGCD_Moulinet(),
            new RedMageGCD_Base()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            //new RedMageAbility_IronWill(),
            //new RedMageAbility_FightorFlight(),
            //new RedMageAbility_Requiescat(),
            //new RedMageAbility_SpiritsWithin(),
            //new RedMageAbility_CircleofScorn(),
            //new RedMageAbility_Intervene(),
            new RedMageAbility_AbilityBase()
        };
        public Task<bool> UsePotion()
        {
            return Task.FromResult(false);
        }
    }
}