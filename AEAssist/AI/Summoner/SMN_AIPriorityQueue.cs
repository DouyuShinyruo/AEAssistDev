using AEAssist.AI.Summoner.Ability;
using AEAssist.AI.Summoner.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AEAssist.AI.Summoner
{
    [Job(ClassJobType.Summoner)]
    public class SMN_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>()
        {
            new SMNGCD_SummonCarbuncle(),
            new SMNGCD_TranceBase(),
            new SMNGCD_Aethercharge(),
            new SMNGCD_PetGarudaSlipstream(),
            new SMNGCD_PetIfritCrimson(),
            new SMNGCD_PetBase(),
            new SMNGCD_PetSummon(),
            new SMNGCD_RuinIV(),
            new SMNGCD_Base()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>()
        {
            new SMNAbility_UsePotion(),
            new SMNAbility_Swiftcast(),
            new SMNAbility_SearingLight(),
            new SMNAbility_PetTitanMountainBuster(),
            new SMNAbility_EnergyDrain(),
            new SMNAbility_Deathflare(),
            new SMNAbility_EnkindleBahamut(),
            new SMNAbility_LucidDreaming(),
            new SMNAbility_Fester(),

        };
        public Task<bool> UsePotion()
        {
            return PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().IntPotionId);
        }
    }
}