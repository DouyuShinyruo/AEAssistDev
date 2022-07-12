using ff14bot.Helpers;
using System.Diagnostics;
using System.Windows.Media;

namespace AEAssist.Helper
{
    public static class LogHelper
    {
        [Conditional("DEBUG")]
        public static void Debug(string msg)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().ShowDebugLog)
                return;
            Logging.Write($"[AEAssist DEBUG] {msg}");
        }

        public static void Info(string msg)
        {
            Logging.Write(Colors.GreenYellow, $"[AEAssist Info] {msg}");
        }

        public static void Error(string msg)
        {
            Logging.Write(Colors.DarkRed, $"[AEAssist Error] {msg}");
        }
    }
}