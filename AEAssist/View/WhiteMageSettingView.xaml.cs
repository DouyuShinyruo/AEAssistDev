using AEAssist.Opener;
using ff14bot.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;

namespace AEAssist.View
{
    public partial class WhiteMageSettingView : UserControl
    {
        public WhiteMageSettingView()
        {
            InitializeComponent();

            var whiteMageResPriority = new Dictionary<int, string>
            {
                { 0, "Healer>Tanks>DPS" },
                { 1, "Tanks>Healer>DPS" },
                { 2, "DPS>Tanks>Healer" }
            };
            WhiteMageResPriority.ItemsSource = whiteMageResPriority;
            WhiteMageResPriority.SelectedIndex = SettingMgr.GetSetting<WhiteMageSettings>().WhiteMageResPriority;


            if (OpenerMgr.Instance.JobOpeners.ContainsKey(ClassJobType.WhiteMage))
            {
                ChooseOpener.ItemsSource = OpenerMgr.Instance.JobOpeners[ClassJobType.WhiteMage];
                ChooseOpener.SelectedValue = SettingMgr.GetSetting<WhiteMageSettings>().WhiteMageOpener;
            }
        }

        private void ChooseOpener_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<WhiteMageSettings>().WhiteMageOpener = ChooseOpener.SelectedValue.ToString();
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.WhiteMage] = ChooseOpener.SelectedValue.ToString();
        }

        private void ChooseResPriority_OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            SettingMgr.GetSetting<WhiteMageSettings>().WhiteMageResPriority = int.Parse(WhiteMageResPriority.SelectedValue.ToString());
        }
    }
}