using AEAssist.Properties;
using PropertyChanged;
using System.Collections.Generic;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class GeneralSettings : IBaseSetting
    {
        public bool AutoFinalBurst { get; set; } = true;

        public GeneralSettings()
        {
            Reset();
        }

        public int UserLatencyOffset { get; set; }
        [ValueRange(300, 700)] public int ActionQueueMs { get; set; }
        [ValueRange(1, 2)] public int MaxAbilityTimsInGCD { get; set; }
        public int AnimationLockMs { get; set; }

        public bool EarlyDecisionMode { get; set; }

        public bool OpenTTK { get; set; }
        [ValueRange(1, 30)] public int TimeToKill_TimeInSec { get; set; }
        [ValueRange(200000, 1000000)] public int TTK_IgnoreDamage { get; set; }

        public bool UsePotion { get; set; }
        [ValueRange(1000, 5000)] public long UsePotionCountDown { get; set; }

        public bool ShowGameLog { get; set; }
        public bool ShowDebugLog { get; set; }

        public bool ShowToast { get; set; }

        public bool ShowBattleTime { get; set; }

        public bool UseCombatMessageOverlay { get; set; }

        public int DexPotionId { get; set; }

        public int MindPotionId { get; set; }

        public int StrPotionId { get; set; }

        public int IntPotionId { get; set; }



        public HashSet<string> DotBlacklist { get; set; } = new HashSet<string>();
        public int AutoFinalBurstCheckTime { get; set; } = 7000;

        public bool NextAbilityFirst { get; set; }
        public bool AutoSwitchTriggerLine { get; set; }

        public bool AutoInterrupt { get; set; }

        public bool UseOpener { get; set; } = true;

        public double OverlayScale_X { get; set; } = 1.0f;
        public double OverlayScale_Y { get; set; } = 1.0f;

        public string CombatMessageFontSize { get; set; } = "30";

        public string LanguagePrefer { get; set; }

        public double OverlayPos_X { get; set; } = 60;
        public double OverlayPos_Y { get; set; } = 60;

        [ValueRange(1500, 1900)] public int RegionOfAbility { get; set; } = 1500;

        public void Reset()
        {
            UserLatencyOffset = 50;
            ActionQueueMs = 500;
            MaxAbilityTimsInGCD = 2;
            OpenTTK = true;
            TimeToKill_TimeInSec = 15;
            TTK_IgnoreDamage = 300000;
            UsePotion = false;
            UsePotionCountDown = 1500;
            AnimationLockMs = 500;
            ShowGameLog = true;
            ShowDebugLog = false;
            ShowToast = false;
            EarlyDecisionMode = true;
            AutoFinalBurst = true;
            AutoFinalBurstCheckTime = 7000;

            DexPotionId = 36105; // 5级巧力
            StrPotionId = 36104; // 5级刚力
            NextAbilityFirst = false;
            AutoSwitchTriggerLine = false;
            AutoInterrupt = false;
            OverlayScale_X = 1;
            OverlayScale_Y = 1;
            UseOpener = true;
            LanguagePrefer = string.Empty;
        }

        public void OnLoad()
        {

        }
    }
}