using System;
using System.Collections.Generic;
using UserControl = System.Windows.Controls.UserControl;

namespace AEAssist.View
{
    public partial class PaladinSettingView : UserControl
    {
        public PaladinSettingView()
        {
            InitializeComponent();

            var fightorFlightTiming = new Dictionary<int, string>
            {
                // Any means either 1 or 2 
                { 0, "Any/任意时间" },
                { 1, "Fast Blade/先锋剑之后" },
                { 2, "Riot Blade/暴乱剑之后" }
            };
            FightorFlightTiming.ItemsSource = fightorFlightTiming;
            FightorFlightTiming.SelectedIndex = SettingMgr.GetSetting<PaladinSettings>().FightorFlightTiming;



        }
        private void ChooseFightorFlightTiming_OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            SettingMgr.GetSetting<PaladinSettings>().FightorFlightTiming = int.Parse(FightorFlightTiming.SelectedValue.ToString());
        }


    }
}