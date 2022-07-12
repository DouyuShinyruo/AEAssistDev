using AEAssist.Helper;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AEAssist.View.Hotkey
{
    public class HotkeyManager
    {
        public static HotkeyManager Instance = new HotkeyManager();


        private Dictionary<string, Action> HotkeyName2Actions = new Dictionary<string, Action>();


        private HashSet<IBuiltinHotkey> _builtinHotkeys = new HashSet<IBuiltinHotkey>();

        private List<ff14bot.Managers.Hotkey> Hotkeys = new List<ff14bot.Managers.Hotkey>();
        public void Init()
        {
            HotkeyName2Actions.Clear();
            _builtinHotkeys.Clear();
            var baseType = typeof(IBuiltinHotkey);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;

                var ins = Activator.CreateInstance(type) as IBuiltinHotkey;
                _builtinHotkeys.Add(ins);
                LogHelper.Info("BuiltinHotkey: " + type.Name);
                HotkeyName2Actions[type.Name] = ins.OnHotkeyDown;
            }
        }

        public void AddBuiltInHotkeyDatas(List<HotkeyData> AllHotkeyDatas)
        {
            foreach (var v in _builtinHotkeys)
            {
                var name = v.GetType().Name;
                if (AllHotkeyDatas.Find(data => data.TypeName == name) == default)
                {
                    AllHotkeyDatas.Add(new HotkeyData(name));
                }
            }

            LogHelper.Info("Loaded Hotkeys " + AllHotkeyDatas.Count);
        }

        public void RegisterHotkeys()
        {
            UnRegisterKey();
            var AllHotkeyDatas = SettingMgr.GetSetting<HotkeySetting>().AllHotkeyDatas;
            foreach (var hotkeyData in AllHotkeyDatas)
            {
                if (hotkeyData.Key == Keys.None)
                {
                    continue;
                }

                if (!HotkeyName2Actions.TryGetValue(hotkeyData.TypeName, out var action))
                    continue;
                var hotkey = hotkeyData;
                Hotkeys.Add(ff14bot.Managers.HotkeyManager.Register($"AEAssist_{hotkeyData.TypeName}", hotkey.Key, hotkey.ModifierKey,
                    v =>
                    {
                        action();
                    }));
            }


        }

        public void UnRegisterKey()
        {
            foreach (var v in Hotkeys)
            {
                ff14bot.Managers.HotkeyManager.Unregister(v);
            }
            Hotkeys.Clear();
        }


        public void RefreshName()
        {
            var AllHotkeyDatas = SettingMgr.GetSetting<HotkeySetting>().AllHotkeyDatas;
            foreach (var v in AllHotkeyDatas)
            {
                foreach (var builtin in _builtinHotkeys)
                {
                    if (builtin.GetType().Name != v.TypeName)
                        continue;
                    v.Name = builtin.GetDisplayString();
                    break;
                }
            }
        }

    }
}