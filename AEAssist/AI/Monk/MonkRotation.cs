using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Rotations.Core;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk
{
    [Job(ClassJobType.Monk)]
    public class MonkRotation : IRotation
    {
        public void Init()
        {
            Random rnd = new Random();
            int Timer = rnd.Next(5000, 10000);
            int ThunderClapTimer = rnd.Next(1800, 2100);
            int CloseUpTimer = rnd.Next(800, 1000);
            CountDownHandler.Instance.AddListener(Timer, () => SpellsDefine.FormShift.DoGCD());
            CountDownHandler.Instance.AddListener(Timer - 2000, () => SpellsDefine.Meditation.DoAbility());
            if (!ActionManager.CanCastOrQueue(SpellsDefine.Bootshine.GetSpellEntity().SpellData, Core.Me.CurrentTarget))
            {
                CountDownHandler.Instance.AddListener(CloseUpTimer, () => SpellsDefine.Thunderclap.DoAbility());
            }
            AEAssist.DataBinding.Instance.EarlyDecisionMode = SettingMgr.GetSetting<MonkSettings>().EarlyDecisionMode;
            LogHelper.Info("EarlyDecisionMode: " + AEAssist.DataBinding.Instance.EarlyDecisionMode);
        }

        public Task<bool> PreCombatBuff()
        {
            return Task.FromResult(false);
        }

        public Task<bool> NoTarget()
        {
            return Task.FromResult(false);
        }

        public SpellEntity GetBaseGCDSpell()
        {
            return SpellsDefine.Bootshine.GetSpellEntity();
        }
    }
}