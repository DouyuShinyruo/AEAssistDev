//using Clio.Utilities.Collections;

using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Gamelog;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using AEAssist.Utilities.CombatMessages;
using AEAssist.View;
using AEAssist.View.OverlayManager;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using TreeSharp;
using HotkeyManager = AEAssist.View.Hotkey.HotkeyManager;

namespace AEAssist
{
    public class Entry
    {
        public const string Path = @"Routines\AEAssist";

        private static MainWindow _form;

        private static MainWindow Form
        {
            get
            {
                if (_form != null) return _form;
                _form = new MainWindow();
                _form.Closed += (sender, args) => { _form = null; };
                return _form;
            }
        }

        private ClassJobType CurrentJob { get; set; }
        private ushort CurrentZone { get; set; }

        public void Initialize()
        {
            LogHelper.Info("Init....Version " + ConstValue.ProjectVersion);
            try
            {
                HotkeyManager.Instance.Init();
                SettingMgr.Instance.InitSetting();
                LanguageHelper.Init();
                DataHelper.Init();
                RotationManager.Instance.Init();
                HookBehaviors();
                SettingMgr.GetSetting<HotkeySetting>().RegisHotkey();
                SettingMgr.GetSetting<HotkeySetting>().ResetHotkeyName();
                AEGamelogManager.Instance.Init();
                OverlayManager.Instance.Init();
                AISpellQueueMgr.Instance.Init();
                PotionHelper.Init();
                // PotionHelper.DebugAllItems();
                AIRoot.Instance.Init();
                AIMgrs.Instance.Init();

                UIHelper.SetToolTipDuration();

                LogHelper.Info("Initialized!");
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }

        }

        public void Pulse()
        {
            RotationManager.Instance.CheckChangeJob();
            OverlayManager.Instance.SwitchJob();
            WorldHelper.CheckZone();
            GamelogManager.Pulse();
            SettingMgr.Instance.AutoSave();
            CombatMessageManager.UpdateDisplayedMessage();
            // MeleePosition.Intance.GetPriority();
        }

        public void Shutdown()
        {
            LogHelper.Info("...ShutDown");
            AEGamelogManager.Instance.Close();
            OverlayManager.Instance.Close();
            SettingMgr.GetSetting<HotkeySetting>().UnRegisterKey();
        }

        public void OnButtonPress()
        {
            Form.Show();
        }


        #region Behavior Composites

        public void HookBehaviors()
        {
            TreeHooks.Instance.ReplaceHook("Rest", RestBehavior);
            TreeHooks.Instance.ReplaceHook("PreCombatBuff", PreCombatBuffBehavior);
            TreeHooks.Instance.ReplaceHook("Pull", PullBehavior);
            TreeHooks.Instance.ReplaceHook("Heal", HealBehavior);
            TreeHooks.Instance.ReplaceHook("CombatBuff", CombatBuffBehavior);
            TreeHooks.Instance.ReplaceHook("Combat", CombatBehavior);
            TreeHooks.Instance.ReplaceHook("PullBuff", PullBuffBehavior);
        }

        public Composite RestBehavior { get; } = new TreeSharp.Action();

        public Composite PreCombatBuffBehavior { get; } = new TreeSharp.Action();


        public Composite PullBehavior { get; } = new TreeSharp.Action();

        public Composite HealBehavior
        {
            get
            {
                return
                    new ActionRunCoroutine(ctx => RotationManager.Instance.Update());
            }
        }

        public Composite CombatBuffBehavior { get; } = new TreeSharp.Action();

        public Composite CombatBehavior
        {
            get
            {
                return
                    new ActionRunCoroutine(ctx => RotationManager.Instance.Update());
            }
        }
        public Composite PullBuffBehavior { get; } = new TreeSharp.Action();
        #endregion Behavior Composites
    }
}
