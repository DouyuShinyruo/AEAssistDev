using PropertyChanged;
using System;

namespace AEAssist.TriggerCond
{
    [Trigger("AfterBattleStart 战斗开始后多少秒",
        Tooltip: "How many seconds have elapsed since the start of the battle\n" +
                "战斗开始后多少秒",
        ParamTooltip = "[time in sec]\n[多少秒]",
        Example = "20")]
    [AddINotifyPropertyChangedInterface]
    public class TriggerCond_AfterBattleStart : ITriggerCond
    {
        public int Time { get; set; }

        public void WriteFromJson(string[] values)
        {
            if (!int.TryParse(values[0], out var time)) throw new Exception($"{values[0]}Error!\n");

            Time = time;
            Check();
        }

        public string[] Pack2Json()
        {
            return new string[]
            {
                this.Time.ToString()
            };
        }

        public void Check()
        {
            if (Time < 0) throw new Exception("Out of range!");
            if (Time > 2000) throw new Exception($"Time is too large! : Sec: {Time}");
        }
    }
}