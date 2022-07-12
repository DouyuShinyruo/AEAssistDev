using AEAssist.AI;
using AEAssist.AI.Machinist;
using AEAssist.Define;
using AEAssist.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AEAssist.View.Overlay
{
    public partial class MCHOverlayWindow : UserControl
    {
        public MCHOverlayWindow()
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

        private void UseTactician_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Tactician.GetSpellEntity();
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

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.OverlayManager.Instance.Close();
        }


        private void UseQueueOverdrive_OnClick(object sender, RoutedEventArgs e)
        {
            var spellId = MCHSpellHelper.GetQueenOverdrive();
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = spellId;
        }

        private void UseWildfire_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Wildfire.GetSpellEntity();
        }

        private void UseDetonator_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Detonator.GetSpellEntity();
        }


        private void UseAutomatonQueen_OnClick(object sender, RoutedEventArgs e)
        {
            var spellId = MCHSpellHelper.GetAutomatonQueen();
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = spellId;
        }

        private void UseHyperCharge_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Hypercharge.GetSpellEntity();
        }

        private void UseHeadGraze_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.HeadGraze.GetSpellEntity();
        }
    }
}