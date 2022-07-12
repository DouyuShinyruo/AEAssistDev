using AEAssist.AI.Machinist.Ability;
using AEAssist.AI.Machinist.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist
{
    [Job(ClassJobType.Machinist)]
    public class MCH_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>
        {
            new MCHGCD_ReassembleGCD(),
            new MCHGCD_UnderHyperCharge(),
            new MCHGCD_Drill(),
            new MCHGCD_AirAnchor(),
            new MCHGCD_ChainSaw(),
            new MCHGCD_BaseCombo()
        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>
        {
            new MCHAbility_UsePotion(),
            new MCHAbility_Reassemble(),
            new MCHAbility_WildFire(),
            new MCHAbility_HyperCharge(),
            new MCHAbility_BarrelStabilizer(),
            new MCHAbility_UseBattery(),
            new MCHAbility_FinalBurst(),
            new MCHAbility_UseGaussRound()
        };

        public Task<bool> UsePotion()
        {
            return PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId);
        }
    }
}