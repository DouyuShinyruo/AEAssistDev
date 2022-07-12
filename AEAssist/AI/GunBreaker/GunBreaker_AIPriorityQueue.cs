using AEAssist.AI.GunBreaker.Ability;
using AEAssist.AI.GunBreaker.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Gunbreaker
{
    [Job(ClassJobType.Gunbreaker)]
    public class GunBreaker_AIPriority : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new UpdatePos(),
            //new GunBreakerGCD_DoubleDown(),
            new GunBreakerGCD_SecondaryCombo(),
            new GunBreakerGCD_SonicBreak(),
            new GunBreakerGCD_DoubleDown(),
            new GunBreakerGCD_BurstStrike(),
            new GunBreakerGCD_BaseGCDCombo()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new UpdatePos(),
            new GunBreakerAbility_NoMercy(),
            new GunBreakerAbility_BowShock(),
            new GunBreakerAbility_Continuation(),
            //new GunBreakerAbility_NoMercy(),
            //new GunBreakerAbility_RoughDivide(),
            new GunBreakerAbility_Bloodfest(),
            new GunBreakerAbility_BlastingZone(),
            //new GunBreakerAbility_BowShock(),
            new GunBreakerAbility_RoughDivide()
        };

        public async Task<bool> UsePotion()
        {
            return await PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId);
        }
    }
}