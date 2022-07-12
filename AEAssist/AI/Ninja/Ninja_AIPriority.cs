using AEAssist.AI.Ninja.Ability;
using AEAssist.AI.Ninja.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja
{
    [Job(ClassJobType.Ninja)]
    public class Ninja_AIPriority : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new NinjaGCD_BaseGCD(),
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new NinjaAbility_TrickAttack(),

        };
        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId);
        }
    }
}