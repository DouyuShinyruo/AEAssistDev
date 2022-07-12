using AEAssist.View;
using PropertyChanged;
using System;
using System.Collections.Generic;

namespace AEAssist.TriggerCond
{
    [Trigger("AfterOtherGroup 另一组触发后", Tooltip = "After another group triggered\n等另一组触发了之后",
        ParamTooltip = "[Other trigger's group id(Has ComplexMode)],[Time in sec]\nComplexMode:\n\tAnd:[Trigger1&Trigger2&...]/Or:[Trigger1|Trigger2|Trigger3....]\n" +
                       "[另一组触发器Id(有复杂模式ComplexMode)],[过了多少秒]",
        Example = "group5,30\n\tgroup1&group2,5\n\tgroup3|group4|group6,10")]
    [AddINotifyPropertyChangedInterface]
    [GUIDefault]
    public class TriggerCond_AfterOtherTrigger : ITriggerCond
    {
        public string TriggerId { get; set; }
        public int Time { get; set; }
        public int Complex { get; set; } // 0 = false,1 = and,2 = or
        public List<string> ComplexTriggers { get; set; } = new List<string>();

        public void WriteFromJson(string[] values)
        {
            if (values[0].Contains("&") && values[0].Contains("|"))
            {
                throw new Exception($"[&][|]Cant use together: {values[0]}!\n");
            }

            if (values[0].Contains("&"))
            {
                Complex = 1;
                ComplexTriggers.AddRange(values[0].Split('&'));
            }
            else if (values[0].Contains("|"))
            {
                Complex = 2;
                ComplexTriggers.AddRange(values[0].Split('|'));
            }
            else
            {
                Complex = 0;
                TriggerId = values[0];
            }
#if Trigger
            if (Complex == 0)
            {
                if (!AETriggers.DataBinding.Instance.GroupIds.Contains(TriggerId))
                    throw new Exception($"Id not found: {values[0]}!\n");
            }
            else
            {
                if (ComplexTriggers.Count == 0)
                    throw new Exception($"ComplexTriggers format error! : {values[0]}!\n");
                foreach (var v in ComplexTriggers)
                {
                    if (!AETriggers.DataBinding.Instance.GroupIds.Contains(v))
                        throw new Exception($"Id not found: {values[0]}!\n");
                }
            }
#endif

            if (!int.TryParse(values[1], out var time)) throw new Exception($"{values[1]}Error!\n");

            Time = time;
            if (Time < 0) throw new Exception("out of range!");
            if (Time > 2000) throw new Exception($"Time is too large! : Sec: {Time}");
        }

        public string[] Pack2Json()
        {
            var str1 = TriggerId;
            if (Complex == 1)
            {
                foreach (var v in ComplexTriggers)
                {
                    str1 += v + "&";
                }

                str1 = str1.Remove(str1.Length - 1);
            }
            else if (Complex == 2)
            {
                foreach (var v in ComplexTriggers)
                {
                    str1 += v + "|";
                }

                str1 = str1.Remove(str1.Length - 1);
            }

            return new string[]
            {
                str1,
                Time.ToString()
            };
        }

        public void Check()
        {

        }
    }
}