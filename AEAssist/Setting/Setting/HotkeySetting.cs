using AEAssist.Helper;
using AEAssist.View.Hotkey;
using AEAssist.View.Hotkey.BuiltinHotkeys;
using PropertyChanged;
using System;
using System.Collections.Generic;
using HotkeyManager = AEAssist.View.Hotkey.HotkeyManager;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class HotkeySetting : IBaseSetting
    {
        private bool _UseHotkey = true;

        public HotkeySetting()
        {
            Reset();
        }

        public string StopBtnName { get; set; }

        public string MoveBtnName { get; set; }

        public string CloseBuffBtnName { get; set; }

        public List<HotkeyData> AllHotkeyDatas { get; set; } = new List<HotkeyData>();

        public bool UseHotkey
        {
            get => _UseHotkey;
            set
            {
                _UseHotkey = value;
                RegisHotkey();
                ResetHotkeyName();
            }
        }

        public void Reset()
        {
            UseHotkey = true;
            if (AllHotkeyDatas == null)
                AllHotkeyDatas = new List<HotkeyData>();
            foreach (var v in AllHotkeyDatas)
            {
                v.Reset();
            }
            ResetHotkeyName();
        }

        public void OnLoad()
        {
            HotkeyManager.Instance.AddBuiltInHotkeyDatas(AllHotkeyDatas);
        }

        public HotkeyData GetHotkeyData(string name)
        {
            return this.AllHotkeyDatas.Find(v => v.Name == name);
        }

        public HotkeyData GetHotkeyDataByTypeName(string type)
        {
            return this.AllHotkeyDatas.Find(v => v.TypeName == type);
        }

        public void ResetHotkeyName()
        {
            if (UseHotkey)
            {
                StopBtnName = $"{Language.Instance.Toggle_Stop} [{GetHotkeyDataByTypeName(nameof(Stop))?.GetDisplayString()}]";
                MoveBtnName = $"{Language.Instance.Toggle_Move} [{GetHotkeyDataByTypeName(nameof(Move))?.GetDisplayString()}]";
                CloseBuffBtnName = $"{Language.Instance.Toggle_BurstOff} [{GetHotkeyDataByTypeName(nameof(Burst))?.GetDisplayString()}]";
            }
            else
            {
                StopBtnName = Language.Instance.Toggle_Stop;
                MoveBtnName = Language.Instance.Toggle_Move;
                CloseBuffBtnName = Language.Instance.Toggle_BurstOff;
            }
        }

        public void RegisHotkey()
        {
            try
            {
                UnRegisterKey();
                if (!UseHotkey)
                    return;
                HotkeyManager.Instance.RegisterHotkeys();

            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
        }

        public void UnRegisterKey()
        {
            HotkeyManager.Instance.UnRegisterKey();
        }
    }
}