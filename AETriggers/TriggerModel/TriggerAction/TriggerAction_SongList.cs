using AEAssist.View;
using PropertyChanged;
using System;
using System.Collections.Generic;

namespace AEAssist.TriggerAction
{
    [Trigger("BRD/SongList 诗人歌单", Tooltip = "If the first song of the song list is the same as the current song, then it will be counted from the current song." +
                                  "Otherwise, the song list will start from the next song." +
                                  "If all songs in the song list are sung and the battle is not over, the default settings will be used for subsequent songs",
        ParamTooltip = "Song number (1 for MB, 2 for AP, 3 for WM): duration (number of seconds), separator is |",
        Example = "3:43|1:42|2:40")]
    [AddINotifyPropertyChangedInterface]
    [GUIDefault]
    public class TriggerAction_SongList : ITriggerAction
    {
        public List<int> Durations { get; set; } = new List<int>();
        public List<int> SongIndex { get; set; } = new List<int>();

        public void WriteFromJson(string[] values)
        {
            if (string.IsNullOrEmpty(values[0]))
                throw new Exception("Error!");

            var strs = values[0].Split('|');
            if (strs == null || strs.Length == 0)
                throw new Exception("Error!");

            foreach (var config in strs)
            {
                var con = config.Split(':');
                if (con == null || con.Length != 2)
                    throw new Exception($"{config} params Error!");
                if (!int.TryParse(con[0], out var index)
                    || !int.TryParse(con[1], out var duration))
                    throw new Exception($"{config} format Error!");

                SongIndex.Add(index);
                Durations.Add(duration);
            }
        }

        public string[] Pack2Json()
        {
            var str = string.Empty;
            for (int i = 0; i < SongIndex.Count; i++)
            {
                str += $"{SongIndex[i]}:{Durations[i]}|";
            }

            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return new string[]
            {
                str
            };
        }

        public void Check()
        {

        }
    }
}