using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AEAssist.View.Overlay
{
    public partial class PaladinOverlayWindow : UserControl
    {
        public PaladinOverlayWindow()
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


        public void UseDivineVeil_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.DivineVeil.GetSpellEntity();
        }
        public void UseHallowedGround_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.HallowedGround.GetSpellEntity();
        }
        public void UseSheltron_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Sheltron.GetSpellEntity();
        }
        public void UseSentinel_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Sentinel.GetSpellEntity();
        }
        public void UseRequiescat_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Requiescat.GetSpellEntity();
        }
        public void UseInterject_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Interject.GetSpellEntity();
        }
        public void UseProvoke_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Provoke.GetSpellEntity();
        }
        public void UseRampart_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Rampart.GetSpellEntity();
        }
        public void UseReprisal_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Reprisal.GetSpellEntity();
        }
        public void UseShirk_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Shirk.GetSpellEntity();
        }

        public void UseLowBlow_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.LowBlow.GetSpellEntity();
        }
    }
}