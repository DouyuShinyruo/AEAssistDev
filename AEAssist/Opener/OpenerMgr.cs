// -----------------------------------
// 
// 模块说明：OpenerMgr.cs
// 
// 创建人员：AE
// 创建日期：2022-04-14
// -----------------------------------

using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.Opener
{

    public class OpenerData
    {
        public string Name { get; set; }
    }


    public class OpenerMgr
    {
        public static OpenerMgr Instance = new OpenerMgr();

        public Dictionary<(ClassJobType, int, string), IOpener> AllOpeners = new Dictionary<(ClassJobType, int, string), IOpener>();

        public Dictionary<ClassJobType, List<OpenerData>> JobOpeners = new Dictionary<ClassJobType, List<OpenerData>>();

        public const string DefaultName = "Default";

        public Dictionary<ClassJobType, string> SpecifyOpenerByName = new Dictionary<ClassJobType, string>();


        public OpenerMgr()
        {
            AllOpeners.Clear();
            var baseType = typeof(IOpener);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;
                var attrs = type.GetCustomAttributes(typeof(OpenerAttribute), false);
                if (attrs.Length == 0)
                {
                    LogHelper.Error($"Opener class [{type}] need OpenerAttribute");
                    continue;
                }

                var attr = attrs[0] as OpenerAttribute;
                var opener = Activator.CreateInstance(type) as IOpener;
                var openerKey = (attr.ClassJobType, attr.Level, attr.Name);
                AllOpeners[openerKey] = opener;

                if (!JobOpeners.ContainsKey(attr.ClassJobType))
                    JobOpeners[attr.ClassJobType] = new List<OpenerData>();
                JobOpeners[attr.ClassJobType].Add(new OpenerData
                {
                    Name = attr.Name
                });

                LogHelper.Info($"Load Opener: {attr.ClassJobType} Level: {attr.Level} Name {attr.Name}");
            }
        }

        public async Task<bool> UseOpener(ClassJobType classJobType)
        {
            var battleData = AIRoot.GetBattleData<BattleData>();

            if (!SpecifyOpenerByName.TryGetValue(classJobType, out var name))
            {
                name = DefaultName;
            }

            if (!AllOpeners.TryGetValue((classJobType, Core.Me.ClassLevel, name), out var opener))
                return false;

            if (battleData.OpenerIndex == 0)
            {
                var ret = opener.Check();
                if (ret < 0)
                {
                    LogHelper.Debug($"Opener Check: {opener.GetType().Name} ret: {ret}");
                    return false;
                }
                LogHelper.Debug($"Use Opener: {opener.GetType().Name}");
            }


            if (opener.Openers.Count < battleData.OpenerIndex) return false;

            var spellQueue = AIRoot.GetBattleData<SpellQueueData>();

            if (!await spellQueue.ApplySlot())
            {
                if (opener.Openers.Count <= battleData.OpenerIndex)
                {
                    battleData.OpenerIndex++;
                    return false;
                }

                var action = opener.Openers[battleData.OpenerIndex];


                var slot = ObjectPool.Instance.Fetch<SpellQueueSlot>();
                action.Invoke(slot);
                LogHelper.Debug($"opener running: Index {battleData.OpenerIndex} GCDSpellId: {slot.GetGCDSpell()}");
                spellQueue.Add(slot);
                battleData.OpenerIndex++;
            }


            return true;
        }
    }
}