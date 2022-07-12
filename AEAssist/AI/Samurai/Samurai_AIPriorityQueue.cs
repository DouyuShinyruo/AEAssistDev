using AEAssist.AI.Samurai.Ability;
using AEAssist.AI.Samurai.GCD;
using AEAssist.Helper;
using ff14bot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai
{
    [Job(ClassJobType.Samurai)]
    public class Samurai_AIPriorityQueue : IAIPriorityQueue
    {
        public List<IAIHandler> GCDQueue { get; } = new List<IAIHandler>
        {
            // new SamuraiGCD_AoERotations(),
            new SamuraiGCD_OddMinuteBurst(),
            // new SamuraiGCD_EvenMinutesBurst(),
            // new SamuraiGCD_Fillers(),
            new SamuraiGCD_MidareSetsugekka(),
            new SamuraiGCD_KaeshiSetsugekka(),
            new SamuraiGCD_Higanbana(),
            new SamuraiGCD_CoolDownPhase(),

        };

        public List<IAIHandler> AbilityQueue { get; } = new List<IAIHandler>
        {
            new SamuraiAbility_HissatsuShinten(),
            // new SamuraiAbility_HissatsuKyuten(),
            // new SamuraiGCD_Dot(),
            // new SamuraiGCD_OgiNamikiriCombo(),
            // new SamuraiAbility_HissatsuKaiten(),
            // new SamuraiAbility_KaeshiSetsugekka(),
            // new SamuraiAbility_HissatsuSenei(),
            new SamuraiAbility_MeikyoShisui(),
            // new SamuraiAbility_Ikishoten(),
            // new SamuraiAbility_Shoha(),
            // new SamuraiAbility_TsubameGaeshi()
        };

        public Task<bool> UsePotion()
        {
            return PotionHelper.ForceUsePotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId);
        }
    }
}