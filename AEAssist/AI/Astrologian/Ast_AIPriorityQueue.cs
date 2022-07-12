using AEAssist.AI.Astrologian.Ability;
using AEAssist.AI.Astrologian.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian
{
    [Job(ClassJobType.Astrologian)]
    internal class Ast_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            //new SageGCDEgeiro(),
            //new AstGCDAscend(),
            new AstGCDDot(),
            //new SageGcdToxikon(),
            //new SageGcdPhlegma(),
            new AstBaseGCD(),
            //new SageGCDDyskrasia(),
            new AstGCDAspectedBenefic(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new AstAbilityLightspeed(),
            new AstAbilityCelestialIntersection(),
            new AstAbilityEssentialDignity(),
            new AstAbilityExaltation(),
            new AstAbilityAstrodyne(),            
            new AstAbilityDraw(),
            new AstAbilityDivination(),
            new AstAbilityRedraw(),
            new AstAbilityPlay(),
            new AstAbilityHalfPlay(),
            new AstAbilityLucidDreaming(),            
            new AstAbilityMinorArcana(),
            new AstAbilityCrownPlay(),
            new AstAbilityUsePotion(),
        };
        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId);
        }
    }
}
