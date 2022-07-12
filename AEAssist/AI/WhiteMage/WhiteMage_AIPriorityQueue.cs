using AEAssist.AI.WhiteMage.Ability;
using AEAssist.AI.WhiteMage.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage
{
    [Job(ClassJobType.WhiteMage)]
    public class WhiteMage_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            /*new SageGCDEgeiro(),
            new SageGcdDot(),
            new SageGcdToxikon(),
            new SageGcdPhlegma(),
            new SageBaseGCD(),
            new SageGCDDyskrasia(),
            */
            new WhiteMageGCDRaise(),
            new WhiteMageGCDDot(),
            new WhiteMageGCDAfflatusSolace(),
            new WhiteMageGCDRegen(),
            new WhiteMageGCDHoly(),
            new WhiteMageBaseGCD()



        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new WhiteMageAbilityLucidDreaming(),
            new WhiteMageAbilityAssize(),
            new WhiteMageAbilityDivineBenison(),
            //new WhiteMageAbilityThinAir(),
            new WhiteMageAbilityTetragrammaton(),
            //new SageAbilityUsePotion(),
        };
        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId);
        }
    }
}