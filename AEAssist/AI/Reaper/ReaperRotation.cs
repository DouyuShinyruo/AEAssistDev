﻿using System.Threading.Tasks;
using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist
{
    [Rotation(ClassJobType.Reaper)]
    public class ReaperRotation : IRotation
    {
        private long _lastTime;
        private readonly AIRoot AiRoot = AIRoot.Instance;

        private long randomTime;

        public void Init()
        {
            CountDownHandler.Instance.AddListener(1500, () =>
            {
                if (Core.Me.HasTarget && Core.Me.CurrentTarget.CanAttack)
                    return SpellHelper.CastGCD(SpellsDefine.Harpe, Core.Me.CurrentTarget);
                return Task.FromResult(false);
            });
            DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<ReaperSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: "+ DataBinding.Instance.EarlyDecisionMode);
        }
        
        // 战斗之前处理buff的?
        public async Task<bool> PreCombatBuff()
        {
            if (Core.Me.HasAura(AurasDefine.Soulsow))
                return true;
            if (await SpellHelper.CastGCD(SpellsDefine.Soulsow, Core.Me))
            {
                GUIHelper.ShowInfo(Language.Instance.Content_Reaper_PreCombat2,500,false);
                randomTime = 0;
                return true;
            }
            return false;
        }

        public SpellData GetBaseGCDSpell()
        {
            return SpellsDefine.Slice;
        }
    }
}