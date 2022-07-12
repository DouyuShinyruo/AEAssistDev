namespace AEAssist.View.Hotkey.BuiltinHotkeys
{
    public class CrimsonCyclone : IBuiltinHotkey
    {
        public void OnHotkeyDown()
        {
            if (!SettingMgr.GetSetting<SMNSettings>().Crimson)
            {
                SettingMgr.GetSetting<SMNSettings>().Crimson = true;
                return;
            }

            SettingMgr.GetSetting<SMNSettings>().Crimson = false;
        }

        public string GetDisplayString()
        {
            return Language.Instance.Combox_Hotkey_SMN_CrimsonCyclone;
        }
    }
}