﻿using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class MCHSettings : IBaseSetting
    {
        public MCHSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }

        public int StrongGCDCheckTime { get; set; } = 6000;
        public bool WildfireFirst { get; set; }

        public void Reset()
        {
            EarlyDecisionMode = true;
            StrongGCDCheckTime = 6000;
            WildfireFirst = false;
        }

        public void OnLoad()
        {
            
        }
    }
}