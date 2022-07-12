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
    public partial class Dancer_Overlay2 : UserControl
    {
        public Dancer_Overlay2()
        {
            InitializeComponent();


        }

        private void UseClosedPositionPM1_OnClick(object sender, RoutedEventArgs e)
        {
            var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            slot.EnqueueAbility((SpellsDefine.Ending, SpellTargetType.Self));
            slot.EnqueueAbility((SpellsDefine.ClosedPosition, SpellTargetType.PM1));
            AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM1);
        }
        private void UseClosedPositionPM2_OnClick(object sender, RoutedEventArgs e)
        {
            // var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            // slot.EnqueueAbility(SpellsDefine.Ending, SpellTargetType.Self);
            // slot.EnqueueAbility(SpellsDefine.ClosedPosition, SpellTargetType.PM2);
            // AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM2);
        }
        private void UseClosedPositionPM3_OnClick(object sender, RoutedEventArgs e)
        {
            // var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            // slot.EnqueueAbility(SpellsDefine.Ending, SpellTargetType.Self);
            // slot.EnqueueAbility(SpellsDefine.ClosedPosition, SpellTargetType.PM3);
            // AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM3);
        }
        private void UseClosedPositionPM4_OnClick(object sender, RoutedEventArgs e)
        {
            // var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            // slot.EnqueueAbility(SpellsDefine.Ending, SpellTargetType.Self);
            // slot.EnqueueAbility(SpellsDefine.ClosedPosition, SpellTargetType.PM4);
            // AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM4);
        }
        private void UseClosedPositionPM5_OnClick(object sender, RoutedEventArgs e)
        {
            // var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            // slot.EnqueueAbility(SpellsDefine.Ending, SpellTargetType.Self);
            // slot.EnqueueAbility(SpellsDefine.ClosedPosition, SpellTargetType.PM5);
            // AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM5);
        }
        private void UseClosedPositionPM6_OnClick(object sender, RoutedEventArgs e)
        {
            // var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            // slot.EnqueueAbility(SpellsDefine.Ending, SpellTargetType.Self);
            // slot.EnqueueAbility(SpellsDefine.ClosedPosition, SpellTargetType.PM6);
            // AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM6);
        }
        private void UseClosedPositionPM7_OnClick(object sender, RoutedEventArgs e)
        {
            // var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            // slot.EnqueueAbility(SpellsDefine.Ending, SpellTargetType.Self);
            // slot.EnqueueAbility(SpellsDefine.ClosedPosition, SpellTargetType.PM7);
            // AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM7);
        }

        private void UseClosedPositionPM8_OnClick(object sender, RoutedEventArgs e)
        {
            // var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
            // slot.EnqueueAbility(SpellsDefine.Ending, SpellTargetType.Self);
            // slot.EnqueueAbility(SpellsDefine.ClosedPosition, SpellTargetType.PM8);
            // AIRoot.GetBattleData<BattleData>().NextSpellSlot = slot;
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Ending, SpellTargetType.Self);
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.ClosedPosition, SpellTargetType.PM8);
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
