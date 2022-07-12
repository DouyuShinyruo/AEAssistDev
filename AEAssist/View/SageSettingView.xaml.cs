using AEAssist.Opener;
using ff14bot.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;

namespace AEAssist.View
{
    public partial class SageSettingView : UserControl
    {
        public SageSettingView()
        {
            InitializeComponent();

            var sageResPriority = new Dictionary<int, string>
            {
                { 0, "Healer>Tanks>DPS" },
                { 1, "Tanks>Healer>DPS" },
                { 2, "DPS>Tanks>Healer" }
            };
            SageResPriority.ItemsSource = sageResPriority;
            SageResPriority.SelectedIndex = SettingMgr.GetSetting<SageSettings>().SageResPriority;


            if (OpenerMgr.Instance.JobOpeners.ContainsKey(ClassJobType.Sage))
            {
                ChooseOpener.ItemsSource = OpenerMgr.Instance.JobOpeners[ClassJobType.Sage];
                ChooseOpener.SelectedValue = SettingMgr.GetSetting<SageSettings>().SageOpener;
            }
        }

        private void ChooseOpener_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<SageSettings>().SageOpener = ChooseOpener.SelectedValue.ToString();
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Sage] = ChooseOpener.SelectedValue.ToString();
        }

        private void ChooseResPriority_OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            SettingMgr.GetSetting<SageSettings>().SageResPriority = int.Parse(SageResPriority.SelectedValue.ToString());
        }
    }
}