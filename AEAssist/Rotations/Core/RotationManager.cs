using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Utilities.CombatMessages;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.Rotations.Core
{
    public class RotationManager
    {
        public static RotationManager Instance = new RotationManager();

        private ClassJobType _classJobType;

        public Dictionary<ClassJobType, IRotation> AllRotations = new Dictionary<ClassJobType, IRotation>();

        public DefaultRotation DefaultRotation = new DefaultRotation();

        public void Init()
        {
            AllRotations.Clear();
            var baseType = typeof(IRotation);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;
                if (type == typeof(DefaultRotation))
                    continue;
                var attrs = type.GetCustomAttributes(typeof(JobAttribute), false);
                if (attrs.Length == 0)
                {
                    LogHelper.Error($"Rotation class [{type}] need RotationAttribute");
                    continue;
                }

                var attr = attrs[0] as JobAttribute;
                AllRotations[attr.ClassJobType] = Activator.CreateInstance(type) as IRotation;
                LogHelper.Debug("Load Rotation: " + attr.ClassJobType);
            }
        }

        public void CheckChangeJob()
        {
            if (_classJobType != ff14bot.Core.Me.CurrentJob)
            {
                _classJobType = ff14bot.Core.Me.CurrentJob;
                CountDownHandler.Instance.ClearListener();
                GetRotation().Init();
                CombatMessageManager.RegisterMessageStrategiesForClass(ff14bot.Core.Me.CurrentJob);
            }
        }

        private IRotation GetRotation()
        {
            if (AllRotations.TryGetValue(ff14bot.Core.Me.CurrentJob, out var job)) return job;

            return DefaultRotation;
        }



        public async Task<bool> PreCombatBuff()
        {
            if (ff14bot.Core.Me.InCombat) return false;
            if (CountDownHandler.Instance.Start)
                return false;
            AIRoot.Instance.Clear();

            if (ff14bot.Core.Me.HasTarget && ff14bot.Core.Me.CurrentTarget.CanAttack)
                return false;

            if (MovementManager.IsMoving)
                return false;
            return await GetRotation().PreCombatBuff();
        }

        public Task NoTarget()
        {
            return GetRotation().NoTarget();
        }


        public async Task<bool> Update()
        {
            try
            {
                await CountDownHandler.Instance.Update();
                await PreCombatBuff();
                TargetMgr.Instance.Update();
                GroupHelper.UpdateAllies();
                return await AIRoot.Instance.Update();
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }

            return false;
        }


        public SpellEntity GetBaseGCDSpell()
        {
            return GetRotation().GetBaseGCDSpell();
        }
    }
}