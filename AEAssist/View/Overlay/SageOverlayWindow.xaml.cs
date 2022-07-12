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
    public partial class SageOverlayWindow : UserControl
    {
        public Action DragMove;

        public SageOverlayWindow()
        {
            InitializeComponent();
        }

        private void SageOverlayWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // this.DragMove();
            }
        }

        // Surecast
        private void UseSureCast_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Surecast.GetSpellEntity();
        }

        // Sprint
        private void UseSprint_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Sprint.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = false;
        }

        private void UsePotion_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = true;
        }

        // E.Diagnosis
        private void UseEukrasianDiagnosis_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO: wait for AE to implement a method to use E.Diagnosis on target.
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            slot.SetGCDQueue((SpellsDefine.Eukrasia, SpellTargetType.Self),
                (SpellsDefine.EukrasianDiagnosis, SpellTargetType.CurrTarget));
            AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;

        }

        // E.Prognosis
        private void UseEukrasianPrognosis_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO: wait for AE to implement a method to use E.Prognosis
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            slot.SetGCDQueue((SpellsDefine.Eukrasia, SpellTargetType.Self),
                (SpellsDefine.EukrasianPrognosis, SpellTargetType.Self));
            AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;

        }

        // Haima
        private void UseHaima_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Haima.GetSpellEntity();
        }

        // Holos
        private void UseHolos_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Holos.GetSpellEntity();
        }

        // Icarus
        private void UseIcarus_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: find Icarus spell id.
            // AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine..GetSpellEntity();
        }

        // Ixochole
        private void UseIxochole_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Ixochole.GetSpellEntity();
        }

        // Krasis
        private void UseKrasis_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Krasis.GetSpellEntity();
        }

        // Panhaima
        private void UsePanhaima_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Panhaima.GetSpellEntity();
        }

        // Pepsi
        private void UsePepsi_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Pepsis.GetSpellEntity();
        }

        // PhysisII
        private void UsePhysisII_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.PhysisII.GetSpellEntity();
        }

        // Pneuma
        private void UsePneuma_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO: Pneuma Doesn't work for some reason...
            // Use Pneuma on current target.
            LogHelper.Debug("trying to use Pneuma next");
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.Pneuma.GetSpellEntity();
        }

        // Rhizomata
        private void UseRhizomata_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Rhizomata.GetSpellEntity();
        }

        // Taurochole
        private void UseTaurochole_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Taurochole.GetSpellEntity();
        }

        // Zoe
        private void UseZoe_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Zoe.GetSpellEntity();
        }

        // Kerachole
        private void UseKerachole_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Kerachole.GetSpellEntity();
        }

        private void Expander_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove?.Invoke();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.OverlayManager.Instance.Close();
        }
    }
}