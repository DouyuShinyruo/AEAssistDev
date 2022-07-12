using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AEAssist.View.Overlay
{
    /// <summary>
    /// BlackMage_Overlay2.xaml 的交互逻辑
    /// </summary>
    public partial class Bard_Overlay2 : UserControl
    {
        public Bard_Overlay2()
        {
            InitializeComponent();


        }

        private void UseAetherialPM1_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM1);
        }
        private void UseAetherialPM2_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM2);
        }
        private void UseAetherialPM3_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM3);
        }
        private void UseAetherialPM4_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM4);
        }
        private void UseAetherialPM5_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM5);
        }
        private void UseAetherialPM6_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM6);
        }
        private void UseAetherialPM7_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM7);
        }

        private void UseAetherialPM8_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.NaturesMinne, SpellTargetType.PM8);
        }

        private void RefreshParty_OnClick(object sender, RoutedEventArgs e)
        {
            AEAssist.Gamelog.AEGamelogManager.Instance.SayPartyLog(() =>
            {
                this.Dispatcher.Invoke(Refresh);
            });


        }

        public void Refresh()
        {
            string GetName(SpellTargetType index)
            {
                Gamelog.AEGamelogManager.Instance.Index2Characters.TryGetValue((int)index - 1, out var chara);
                if (!string.IsNullOrEmpty(chara))
                    return chara;
                return "NULL";
            }

            try
            {

                this.PM1.Text = GetName(SpellTargetType.PM1);
                this.PM2.Text = GetName(SpellTargetType.PM2);
                this.PM3.Text = GetName(SpellTargetType.PM3);
                this.PM4.Text = GetName(SpellTargetType.PM4);
                this.PM5.Text = GetName(SpellTargetType.PM5);
                this.PM6.Text = GetName(SpellTargetType.PM6);
                this.PM7.Text = GetName(SpellTargetType.PM7);
                this.PM8.Text = GetName(SpellTargetType.PM8);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
        }
    }
}
