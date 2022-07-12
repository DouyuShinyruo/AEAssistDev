using ff14bot.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AEAssist.Helper
{
    internal static class DataHelper
    {
        public static Dictionary<uint, string> BossDictionary;


        static DataHelper()
        {
            var bossFile = $@"{Entry.Path}\Resources\BossDictionary.json";

            var bosses = File.ReadAllText(bossFile);
            try
            {
                BossDictionary =
                    new Dictionary<uint, string>(JsonConvert.DeserializeObject<Dictionary<uint, string>>(bosses));
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
                BossDictionary = new Dictionary<uint, string>();
            }


            LogHelper.Info("Load BossCount: " + BossDictionary?.Count);
        }

        public static void Init()
        {
        }

        public static bool IsBoss(this GameObject unit)
        {
            return unit != null &&
                   (BossDictionary.ContainsKey(unit.NpcId) || unit.EnglishName.Contains("Dummy"));
        }

        public static bool IsDummy(this GameObject unit)
        {
            return unit != null && unit.EnglishName.Contains("Dummy");
        }
    }
}