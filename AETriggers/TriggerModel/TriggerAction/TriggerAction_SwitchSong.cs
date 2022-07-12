using AEAssist.View;
using PropertyChanged;
using System;

namespace AEAssist.TriggerAction
{
    [Trigger("BRD/SwitchSong 诗人歌曲控制", Tooltip = "Immediately switch the song. (1 for MB, 2 for AP, 3 for WM). -1=toggle off the song,0 =toggle on ")]
    [AddINotifyPropertyChangedInterface]
    public class TriggerAction_SwitchSong : ITriggerAction
    {
        [GUIToolTip("-1~3.\n(1 for MB, 2 for AP, 3 for WM). -1=toggle off the song,0 =toggle on")]
        [GUIIntRange(-1, 3)]
        public int index { get; set; }

        public void WriteFromJson(string[] values)
        {
            if (!int.TryParse(values[0], out var index)) throw new Exception($"{values[0]} Error!");
            this.index = index;
            Check();
        }

        public string[] Pack2Json()
        {
            return new string[]
            {
                index.ToString()
            };
        }

        public void Check()
        {
            if (index < -1 || index > 3)
                throw new Exception($"{index} <-1 or >3!");
        }
    }
}