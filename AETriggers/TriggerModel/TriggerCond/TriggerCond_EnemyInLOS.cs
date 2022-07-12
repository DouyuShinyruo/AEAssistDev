using AEAssist.View;
using PropertyChanged;
using System;

namespace AEAssist.TriggerCond
{
    [Trigger("EnemyIsTargetable 敌人可选中",
        Tooltip = "Specify enemy is targetable\n某个敌人可选中",
        ParamTooltip = "[enemy name(contains) or NpcId],[Time in sec]",
        Example = "10720,3")]
    [AddINotifyPropertyChangedInterface]
    public class TriggerCond_EnemyInLOS : ITriggerCond
    {
        [GUILabel("Name/Id")]
        public string name { get; set; }
        public int delayTime { get; set; }

        public void WriteFromJson(string[] values)
        {
            name = values[0];
            if (string.IsNullOrEmpty(name)) throw new Exception("is null!");

            if (!int.TryParse(values[1], out var delay)) throw new Exception($"{values[1]}Error!\n");
            delayTime = delay;
            Check();
        }

        public string[] Pack2Json()
        {
            return new string[]
            {
                name,
                delayTime.ToString()
            };
        }

        public void Check()
        {
            if (delayTime < 0) throw new Exception("Must >=0 : " + delayTime);
        }
    }
}