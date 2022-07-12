using ff14bot;
using ff14bot.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace AEAssist.Helper
{
    public static class TriggerLineSwitchHelper
    {
        public static string Path = $@"{SettingMgr.SettingPath}\TriggerLine";

        public static Dictionary<(ushort zoneId, uint sub), TriggerLine> AllTriggerLines =
            new Dictionary<(ushort zoneId, uint sub), TriggerLine>();

        public static Dictionary<ushort, TriggerLine> CurrZoneId2TriggerLine = new Dictionary<ushort, TriggerLine>();


        public static void LoadAll()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            AllTriggerLines.Clear();
            var files = Directory.GetFiles(Path, ".*json");

            if (files == null || files.Length == 0) return;

            foreach (var v in files)
            {
                var triggerLineRet = TriggerHelper.LoadTriggerLine(v);
                if (triggerLineRet.Item2 == null)
                {
                    MessageBox.Show($"File: {v} \n" + triggerLineRet.Item1);
                    AllTriggerLines.Clear();
                    CurrZoneId2TriggerLine.Clear();
                    return;
                }

                AllTriggerLines.Add((triggerLineRet.Item2.CurrZoneId, triggerLineRet.Item2.SubZoneId),
                    triggerLineRet.Item2);
                if (triggerLineRet.Item2.SubZoneId == 0)
                    CurrZoneId2TriggerLine.Add(triggerLineRet.Item2.CurrZoneId, triggerLineRet.Item2);
            }

            var str = "Loaded TriggerLine:\n";
            foreach (var v in AllTriggerLines) str += v.Value.Name + "\n";

            MessageBox.Show(str);
        }

        public static void ApplyTriggerLine(ushort zoneId, uint sub)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().AutoSwitchTriggerLine)
                return;
            TriggerLine line = null;

            var key = (zoneId, sub);
            if (!AllTriggerLines.TryGetValue(key, out line))
                if (!CurrZoneId2TriggerLine.TryGetValue(zoneId, out line))
                    return;

            AEAssist.DataBinding.Instance.ChangeTriggerLine(line);
        }

        public static bool CheckTriggerLine(TriggerLine CurrTriggerLine, out string str)
        {
            str = "Loading failed: TriggerLine is NULL!";
            if (CurrTriggerLine == null)
                return false;
            str = "Loading failed: Job Limit!";
            if (CurrTriggerLine.TargetJob != "Any" &&
                CurrTriggerLine.TargetJob != Enum.GetName(typeof(ClassJobType), Core.Me.CurrentJob))
                return false;
            var canUse = Core.Me.CurrentTarget != null && Core.Me.CurrentTarget.EnglishName.Contains("Dummy");

            if (!canUse)
            {
                if (CurrTriggerLine.CurrZoneId == 0)
                    canUse = true;
                else if (CurrTriggerLine.CurrZoneId == WorldHelper.RawZoneId &&
                         CurrTriggerLine.SubZoneId == WorldHelper.SubZoneId)
                    canUse = true;
            }


            if (!canUse)
            {
                str = "Loading failed: Zone Limit!";
                return false;
            }

            return true;
        }
    }
}