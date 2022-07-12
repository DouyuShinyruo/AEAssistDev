using AEAssist.AI;
using AEAssist.Define;
using System.Windows;
using System.Windows.Controls;

namespace AEAssist.View.Overlay
{
    /// <summary>
    /// BlackMage_Overlay2.xaml 的交互逻辑
    /// </summary>
    public partial class BlackMage_Overlay2 : UserControl
    {
        public BlackMage_Overlay2()
        {
            InitializeComponent();
        }

        private void UseAetherialPM1_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.AetherialManipulation, SpellTargetType.PM1);
        }
        private void UseAetherialPM2_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.AetherialManipulation, SpellTargetType.PM2);
        }
        private void UseAetherialPM3_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.AetherialManipulation, SpellTargetType.PM3);
        }
        private void UseAetherialPM4_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.AetherialManipulation, SpellTargetType.PM4);
        }
        private void UseAetherialPM5_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.AetherialManipulation, SpellTargetType.PM5);
        }
        private void UseAetherialPM6_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.AetherialManipulation, SpellTargetType.PM6);
        }
        private void UseAetherialPM7_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.AetherialManipulation, SpellTargetType.PM7);
        }
    }
}
