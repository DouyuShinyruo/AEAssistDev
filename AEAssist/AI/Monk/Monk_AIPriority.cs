using AEAssist.AI.Monk.Ability;
using AEAssist.AI.Monk.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk
{
    [Job(ClassJobType.Monk)]
    public class Monk_AIPriority : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new MonkGCD_MasterfulBlitz(),
            new MonkGCD_PerfectBalanceGCD(),
            new MonkGCD_FormlessFistGCD(),
            new MonkGCD_BaseGCD(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new MonkAbility_RiddleOfFire(),
            new MonkAbility_PerfectBalance(),
            new MonkAbility_LazyPerfectBalance(),
            new MonkAbility_Brotherhood(),
            new MonkAbility_ChakraAttacks(),
            new MonkAbility_TrueNorth(),
            new MonkAbility_RiddleOfWind(),
            new MonkAbility_UsePotion(),
            // new MonkAbility_SetPosition(),
        };

        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId);
        }
    }
}