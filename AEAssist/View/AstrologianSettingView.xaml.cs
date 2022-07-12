using AEAssist.Opener;
using ff14bot.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;

namespace AEAssist.View
{
    public partial class AstrologianSettingView : UserControl
    {
        public AstrologianSettingView()
        {
            InitializeComponent();

            var astResPriority = new Dictionary<int, string>
            {
                { 0, "Healer>Tanks>DPS" },
                { 1, "Tanks>Healer>DPS" },
                { 2, "DPS>Tanks>Healer" }
            };
            AstResPriority.ItemsSource = astResPriority;
            AstResPriority.SelectedIndex = SettingMgr.GetSetting<AstSettings>().AstResPriority;


            if (OpenerMgr.Instance.JobOpeners.ContainsKey(ClassJobType.Astrologian))
            {
                ChooseOpener.ItemsSource = OpenerMgr.Instance.JobOpeners[ClassJobType.Astrologian];
                ChooseOpener.SelectedValue = SettingMgr.GetSetting<AstSettings>().AstOpener;
            }
        }

        private void ChooseOpener_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<AstSettings>().AstOpener = ChooseOpener.SelectedValue.ToString();
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Astrologian] = ChooseOpener.SelectedValue.ToString();
        }

        private void ChooseResPriority_OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            SettingMgr.GetSetting<AstSettings>().AstResPriority = int.Parse(AstResPriority.SelectedValue.ToString());
        }
    }
}