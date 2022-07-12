using AEAssist.Helper;
using System;
using System.Collections.Generic;

namespace AEAssist.TriggerSystem
{
    public class TriggerSystemMgr
    {
        public static TriggerSystemMgr Instance = new TriggerSystemMgr();

        private readonly Dictionary<Type, ITriggerActionHandler> AllActionHandlers =
            new Dictionary<Type, ITriggerActionHandler>();

        private readonly Dictionary<Type, ITriggerCondHandler> AllCondHandlers =
            new Dictionary<Type, ITriggerCondHandler>();

        public TriggerSystemMgr()
        {
            var baseType = typeof(ITriggerCondHandler);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;

                var handler = Activator.CreateInstance(type) as ITriggerCondHandler;
                AllCondHandlers.Add(handler.GetCondType(), Activator.CreateInstance(type) as ITriggerCondHandler);
            }

            baseType = typeof(ITriggerActionHandler);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;

                var handler = Activator.CreateInstance(type) as ITriggerActionHandler;
                AllActionHandlers.Add(handler.GetActionType(),
                    Activator.CreateInstance(type) as ITriggerActionHandler);
            }
        }

        public bool AllowTriggers { get; set; } = true;

        public bool HandleTriggers(Trigger trigger)
        {
            if (trigger.TriggerConds.Count > 0)
                foreach (var v in trigger.TriggerConds)
                    if (!HandleCond(v))
                        return false;

            LogHelper.Info("Hit Trigger: " + trigger.Id);

            foreach (var v in trigger.TriggerActions) HandleAction(v);

            return true;
        }

        public bool HandleCond(ITriggerCond cond)
        {
            if (!AllCondHandlers.TryGetValue(cond.GetType(), out var handler))
            {
                LogHelper.Error($"there is no cond handler for {cond.GetType().FullName}");
                return false;
            }

            try
            {
                return handler.Handle(cond);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
                return false;
            }
        }

        public void HandleAction(ITriggerAction action)
        {
            if (!AllowTriggers)
                return;
            if (!AllActionHandlers.TryGetValue(action.GetType(), out var handler))
            {
                LogHelper.Error($"there is no action handler for {action.GetType().FullName}");
                return;
            }

            LogHelper.Info("HandleAction : " + action.GetType().Name);
            try
            {
                handler.Run(action);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
        }
    }
}