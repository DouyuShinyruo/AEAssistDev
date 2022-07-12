using ff14bot;
using System;
using System.Windows;
using System.Windows.Media;

namespace AEAssist.Helper
{
    public static class GUIHelper
    {
        private static long targetShowTime;

        public static void OpenGUI()
        {
            // if (GUI == null || GUI.IsDisposed || GUI.Disposing)
            //     GUI = new GUI();
            // GUI.Show();
        }

        public static void Close()
        {
            // GUI.Close();
        }

        public static void ShowInfo(string msg, int time = 0, bool check = true)
        {
            if (check && TimeHelper.Now() < targetShowTime)
                return;
            if (time > 0)
                targetShowTime = TimeHelper.Now() + time;
            DebugCenter.Intance.ShowMsg(msg, check);
        }

        public static void ShowMessageBox(string msg)
        {
            MessageBox.Show(msg);
        }

        public static void ShowToast(string msg, int time = 1500)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().ShowToast)
                return;
            {
                Core.OverlayManager.AddToast(() => msg,
                    TimeSpan.FromSeconds(time),
                    Colors.Green,
                    Colors.Black,
                    new FontFamily("Consolas"));
            }
        }
    }
}