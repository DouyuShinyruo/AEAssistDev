using AEAssist.Helper;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AEAssist.View
{
    public partial class GeneralSettingView : UserControl
    {
        private ObservableCollection<DotBlackList_Data> Ob_DotBlackList = new ObservableCollection<DotBlackList_Data>();

        public GeneralSettingView()
        {
            InitializeComponent();


            SwitchLan.ItemsSource = LanguageHelper.LanOptions;
            SwitchLan.SelectedValue = AEAssist.Language.Instance.LanType;


            Dex_ChoosePotion.ItemsSource = PotionHelper.DexPotions;
            Dex_ChoosePotion.SelectedValue = SettingMgr.GetSetting<GeneralSettings>().DexPotionId;

            Str_ChoosePotion.ItemsSource = PotionHelper.StrPotions;
            Str_ChoosePotion.SelectedValue = SettingMgr.GetSetting<GeneralSettings>().StrPotionId;

            Mind_ChoosePotion.ItemsSource = PotionHelper.MindPotions;
            Mind_ChoosePotion.SelectedValue = SettingMgr.GetSetting<GeneralSettings>().MindPotionId;

            Int_ChoosePotion.ItemsSource = PotionHelper.IntPotions;
            Int_ChoosePotion.SelectedValue = SettingMgr.GetSetting<GeneralSettings>().IntPotionId;

            RefreshDotBlackList();

            var dir = Directory.CreateDirectory(TriggerLineSwitchHelper.Path);

            //Path.Content = dir.FullName;
        }


        private void SwitchLan_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LanguageHelper.SwitchLan((string)SwitchLan.SelectedValue);
        }

        private void Dex_ChoosePotion_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<GeneralSettings>().DexPotionId = (int)Dex_ChoosePotion.SelectedValue;
        }

        private void Str_ChoosePotion_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<GeneralSettings>().StrPotionId = (int)Str_ChoosePotion.SelectedValue;
        }

        private void Mind_ChoosePotion_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<GeneralSettings>().MindPotionId = (int)Mind_ChoosePotion.SelectedValue;
        }

        private void Int_ChoosePotion_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingMgr.GetSetting<GeneralSettings>().IntPotionId = (int)Int_ChoosePotion.SelectedValue;
        }

        private void DotBlackList_Add_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DotBlackList_Input.Text))
                return;
            if (SettingMgr.GetSetting<GeneralSettings>().DotBlacklist.Add(DotBlackList_Input.Text))
                RefreshDotBlackList();
        }

        private void DotBlackList_Remove_OnClick(object sender, RoutedEventArgs e)
        {
            var currSelect = DotBlackList.SelectedItem as ListBoxItem;

            if (currSelect == null)
                return;
            if (SettingMgr.GetSetting<GeneralSettings>().DotBlacklist.Remove(currSelect.Content.ToString()))
                RefreshDotBlackList();
        }

        private void RefreshDotBlackList()
        {
            DotBlackList.SelectedItem = null;
            DotBlackList.Items.Clear();


            foreach (var v in SettingMgr.GetSetting<GeneralSettings>().DotBlacklist)
                DotBlackList.Items.Add(new ListBoxItem
                {
                    Content = v,
                    Foreground = Brushes.Aqua,
                    Height = 25,
                    Width = 100,
                    FontSize = 10,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                });
        }


        private void LoadTriggerLine_OnClick(object sender, RoutedEventArgs e)
        {
            TriggerLineSwitchHelper.LoadAll();
        }

        private class DotBlackList_Data
        {
            public string Name { get; set; }
        }

        private void ApplyScale_OnClick(object sender, RoutedEventArgs e)
        {
            DataBinding.Instance.ApplyScale();
        }
    }
}