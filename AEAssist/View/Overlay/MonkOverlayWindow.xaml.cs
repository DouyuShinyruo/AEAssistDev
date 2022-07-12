using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AEAssist.View.Overlay
{
    public partial class MonkOverlayWindow : UserControl
    {
        public MonkOverlayWindow()
        {
            InitializeComponent();
        }

        private void BardOverlayWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
            }
        }
        private void UseSixsidedStar_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.SixSidedStar.GetSpellEntity();
        }
        private void UseTrueNorth_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.TrueNorth.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = false;
        }
        private void UseMantra_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Mantra.GetSpellEntity();
        }
        private void UseRiddleOfEarth_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.RiddleofEarth.GetSpellEntity();
        }
        private void UsePerfectBalance_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.PerfectBalance.GetSpellEntity();
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
        private void UseBloodbath_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Bloodbath.GetSpellEntity();
        }
        private void UseLegSweep_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.LegSweep.GetSpellEntity();
        }
        private void UseFeint_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Feint.GetSpellEntity();
        }
        private void UseUseSecondwind_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.SecondWind.GetSpellEntity();
        }

        private void UsePotion_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = true;
        }


        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.OverlayManager.Instance.Close();
        }

        private void UseHeadGraze_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.HeadGraze.GetSpellEntity();
        }
    }
}