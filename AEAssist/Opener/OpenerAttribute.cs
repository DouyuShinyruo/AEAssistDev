
using ff14bot.Enums;
using System;

namespace AEAssist.Opener
{
    public class OpenerAttribute : Attribute
    {
        public ClassJobType ClassJobType;
        public int Level;
        public string Name;

        public OpenerAttribute(ClassJobType classJobType, int level, string name = OpenerMgr.DefaultName)
        {
            ClassJobType = classJobType;
            Level = level;
            Name = name;
        }
    }
}