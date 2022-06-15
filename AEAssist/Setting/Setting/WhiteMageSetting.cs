﻿using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot.Enums;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class WhiteMageSettings : IBaseSetting
    {
        public WhiteMageSettings()
        {
            Reset();
        }
        public void Reset()
        {
            EarlyDecisionMode = true;
            Dot_TimeLeft = ConstValue.AuraTick;
            TTK_Aero = 30;
            WhiteMageOpener = "Default";
        }
        public void OnLoad()
        {
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.WhiteMage] = WhiteMageOpener;
            LogHelper.Info($"WhiteMage Opener: {WhiteMageOpener}");
        }
        public int Dot_TimeLeft { get; set; } = ConstValue.AuraTick;
        public int TTK_Aero { get; set; }

        public int LucidDreamingTrigger { get; set; } = ConstValue.LucidDreamingDefaultRefresh;
        public bool LucidDreamingToggle { get; set; } = true;
        public bool SwiftResToggle { get; set; } = true;
        public bool EarlyDecisionMode { get; set; }
        public string WhiteMageOpener { get; set; } = "Default";
        public int WhiteMageResPriority { get; set; } = 0;
    }
}