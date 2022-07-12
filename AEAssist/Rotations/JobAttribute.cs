using ff14bot.Enums;
using System;

namespace AEAssist
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class JobAttribute : Attribute
    {
        public ClassJobType ClassJobType;

        public JobAttribute(ClassJobType classJobType)
        {
            ClassJobType = classJobType;
        }
    }
}