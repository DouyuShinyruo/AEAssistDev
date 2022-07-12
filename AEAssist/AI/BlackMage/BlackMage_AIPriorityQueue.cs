using AEAssist.AI.BlackMage.Ability;
using AEAssist.AI.BlackMage.GCD;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage
{
    [Job(ClassJobType.BlackMage)]
    public class BlackMage_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new BlackMageGCD_Dot(), //done, priority #1
            new BlackMageGCD_Xenoglossy(), //done
            new BlackMageGCD_Fire2(),
            new BlackMageGCD_Fire4(), //done, must before paradox
            new BlackMageGCD_Blizzard4(), //done, before ice paradox
            new BlackMageGCD_Fire3(), //done
            new BlackMageGCD_Paradox(), //done, must after fire4
            new BlackMageGCD_Despair(), //done, must after all fire spells
            new BlackMageGCD_Blizzard3(), //done, must after all fire
            new BlackMageGCD_UmbralSoul(),

        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            // new BlackMageAblity_Manafont(),
            new BlackMageAblity_Amplifier(),
            new BlackMageAblity_Triplecast(),
            new BlackMageAblity_Sharpcast(),
            // new BlackMageAblity_Swiftcast(),
            new BlackMageAblity_Leylines(),
            new BlackMageAblity_Transpose(),
        };
        public Task<bool> UsePotion()
        {
            return Task.FromResult(false);
        }
    }
}