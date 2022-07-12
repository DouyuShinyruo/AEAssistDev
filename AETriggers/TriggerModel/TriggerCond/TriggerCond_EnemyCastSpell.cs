using AEAssist.View;
using MongoDB.Bson.Serialization.Attributes;
using PropertyChanged;
using System;

namespace AEAssist.TriggerCond
{
    [Trigger("EnemyCastSpell 敌人读条技能", Tooltip = "when any enemy casting the spell contains specify name or equal specify id\n某个敌人正在读条含有指定名字或者等于指定id的技能",
        ParamTooltip = "[spell name(contains) or spellId],[Time in sec after the beginning of cast]\n[名字或者id],[从开始读条那一刻过了多少秒]",
        Example = "12345,20\n\tfire,5\n\t掌掴,10")]
    [AddINotifyPropertyChangedInterface]
    public class TriggerCond_EnemyCastSpell : ITriggerCond
    {
        [GUILabel("Name/Id")]
        [GUIToolTip("Example:\nSpell1\n123\nSpell1|Spell2\n1234|Spell2|1236|..etc")]
        public string spellName { get; set; }
        [GUIToolTip("how long after the enemy start casting the specify spell (sec)\n敌人开始读条后过多少秒")]
        public int delayTime { get; set; }

        [BsonIgnore]
        public string[] strs;

        public void WriteFromJson(string[] values)
        {
            spellName = values[0];
            if (string.IsNullOrEmpty(spellName)) throw new Exception("Error!");
            if (!int.TryParse(values[1], out var va)) throw new Exception($"{values[1]}Error!\n");
            if (va < 0) throw new Exception("Must >= 0! :" + va);
            delayTime = va;
        }

        public string[] Pack2Json()
        {
            return new string[]
            {
                spellName,
                delayTime.ToString()
            };
        }

        public void Check()
        {

        }
    }
}