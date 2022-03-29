﻿using System.Linq;
using AEAssist.Define;
using AEAssist.Helper;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class BardSettings : IBaseSetting
    {
        public double RestHealthPercent { get; set; } = 70.0f;
        public int ApexArrowValue { get; set; } = 95; // 绝峰 多少能量就用

        public SongStrategyEnum CurrentSongPlaylist { get; set; }

        public int Songs_WM_TimeLeftForSwitch { get; set; } = 3000;
        public int Songs_MB_TimeLeftForSwitch { get; set; } = 11000;
        public int Songs_AP_TimeLeftForSwitch { get; set; } = 3000;

        public bool UsePeloton { get; set; }

        public bool BuffsDelay2GCD { get; set; } // 起手双团辅延后两个GCD 还是1个

        public int TTK_IronJaws { get; set; } = 28; // 不刷伶牙

        public int PotionId = 36105;

    }
}