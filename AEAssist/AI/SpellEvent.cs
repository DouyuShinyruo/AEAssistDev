using AEAssist.Helper;
using System;
using System.Collections.Generic;

namespace AEAssist.AI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SpellEventAttribute : Attribute
    {
        public uint spellId;

        public SpellEventAttribute(uint id)
        {
            spellId = id;
        }
    }

    public interface ISpellEvent
    {
        void Run(uint spellId);
    }

    public class SpellEventMgr
    {
        public static SpellEventMgr Instance = new SpellEventMgr();


        public Dictionary<uint, ISpellEvent> AllEvents = new Dictionary<uint, ISpellEvent>();

        public SpellEventMgr()
        {
            AllEvents.Clear();
            var baseType = typeof(ISpellEvent);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;
                var attrs = type.GetCustomAttributes(typeof(SpellEventAttribute), false);
                if (attrs.Length == 0)
                {
                    LogHelper.Error($"ISpellEvent class [{type}] need SpellEventAttribute");
                    continue;
                }

                var spellEvent = Activator.CreateInstance(type) as ISpellEvent;
                foreach (SpellEventAttribute attr in attrs) AllEvents[attr.spellId] = spellEvent;
            }
        }

        public void Run(uint id)
        {
            if (!AllEvents.TryGetValue(id, out var spellEvent)) return;

            spellEvent.Run(id);
        }
    }
}