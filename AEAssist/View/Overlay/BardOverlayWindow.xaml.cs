using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using PropertyChanged;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AEAssist.View.Overlay
{
    [AddINotifyPropertyChangedInterface]
    public partial class BardOverlayWindow : UserControl
    {
        public Action DragMove;

        public BardOverlayWindow()
        {
            InitializeComponent();
        }

        private void BardOverlayWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // this.DragMove();
            }
        }

        private void UseTroubadour_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Troubadour.GetSpellEntity();
        }

        private void UseArmsLength_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ArmsLength.GetSpellEntity();
        }

        private void UseSprint_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Sprint.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = false;
        }

        private void UsePotion_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = true;
        }

        private void Expander_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove?.Invoke();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.OverlayManager.Instance.Close();
        }

        private void UseApex_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.ApexArrow.GetSpellEntity();
        }

        private void UseWM_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.TheWanderersMinuet.GetSpellEntity();
        }

        private void UseMB_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.MagesBallad.GetSpellEntity();
        }

        private void UseAP_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ArmysPaeon.GetSpellEntity();
        }

        private void UseBlastArrow_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.BlastArrow.GetSpellEntity();
        }

        private void UseNaturesMinne_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.NaturesMinne.GetSpellEntity();
        }

        private void UseIronJaw_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.IronJaws.GetSpellEntity();
        }

        private void UseHeadGraze_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.HeadGraze.GetSpellEntity();
        }

        private void UseUseSecondwind_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.SecondWind.GetSpellEntity();
        }
    }
}