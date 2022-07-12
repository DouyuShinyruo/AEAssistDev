using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using System.Windows;
using System.Windows.Controls;

namespace AEAssist.View.Overlay
{
    public partial class GunBreakerOverlayWindow : UserControl
    {
        public GunBreakerOverlayWindow()
        {
            InitializeComponent();
        }
        private void UseSprint_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Sprint.GetSpellEntity();
        }

        private void UsePotion_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = true;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.OverlayManager.Instance.Close();
        }

        private void UseRampart_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Rampart.GetSpellEntity();
        }

        private void UseReprisal_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Reprisal.GetSpellEntity(); 
        }

        private void UseSuperbolide_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Superbolide.GetSpellEntity();
        }

        private void UseNebula_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Nebula.GetSpellEntity();
        }

        private void UseCamouflage_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Camouflage.GetSpellEntity();
        }

        private void UseHeartOfCorundum_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.HeartOfCorundum.GetSpellEntity();
        }
    }
}
