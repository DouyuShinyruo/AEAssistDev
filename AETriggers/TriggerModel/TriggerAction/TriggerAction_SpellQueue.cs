using AEAssist.View;
using PropertyChanged;
using System;
using System.Collections.Generic;

namespace AEAssist.TriggerAction
{
    [Trigger("SpellQueue 技能队列", Tooltip = "Let the next used spells follow a queue.The GCD type spells are used first, followed by Ability, and finally Potion.\n" +
                                         "让接下来的技能使用遵循一个队列,先使用GCD队列,其次能力技,最后使用爆发",
        ParamTooltip = "[GCD Queue],[Ability Queue],[Potion(1=Use,0=Not)] (separator is |)",
        Example = "1001|1002|1003,2001|2002,0")]
    [AddINotifyPropertyChangedInterface]
    [GUIDefault]
    public class TriggerAction_SpellQueue : ITriggerAction
    {
        public List<uint> GCDQueue = new List<uint>();
        public List<uint> AbilityQueue = new List<uint>();
        public bool UsePotion;
        public void WriteFromJson(string[] values)
        {
            if (values == null || values.Length != 3)
            {
                throw new Exception("Format Error!" + values.ArrayToString());
            }

            var gcd = values[0].Split('|');
            foreach (var v in gcd)
            {
                if (!uint.TryParse(v, out var gcdSpellId))
                    throw new Exception("Format Error!" + values[0]);
                this.GCDQueue.Add(gcdSpellId);
            }

            var ability = values[1].Split('|');
            foreach (var v in ability)
            {
                if (!uint.TryParse(v, out var abilityId))
                    throw new Exception("Format Error!" + values[1]);
                this.AbilityQueue.Add(abilityId);
            }

            if (!int.TryParse(values[2], out var potion))
            {
                throw new Exception("Format Error!" + values[2]);
            }

            UsePotion = potion == 1;
        }

        public string[] Pack2Json()
        {
            var str1 = string.Empty;
            foreach (var v in GCDQueue)
            {
                str1 += v.ToString() + "|";
            }

            if (str1.Length > 0)
                str1 = str1.Remove(str1.Length - 1);

            var str2 = string.Empty;
            foreach (var v in AbilityQueue)
            {
                str2 += v.ToString() + "|";
            }

            if (str2.Length > 0)
                str2 = str2.Remove(str2.Length - 1);

            var str3 = UsePotion ? "1" : "0";
            return new string[]
            {
                str1,
                str2,
                str3
            };
        }

        public void Check()
        {

        }
    }
}