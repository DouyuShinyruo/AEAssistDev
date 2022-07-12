using AEAssist.Define;
using AEAssist.Gamelog;
using AEAssist.Helper;
using AEAssist.Opener;
using AEAssist.Rotations.Core;
using ff14bot;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI
{
    public interface IBattleData
    {
    }

    public class AIRoot
    {
        public static readonly AIRoot Instance = new AIRoot();

        private readonly Dictionary<Type, IBattleData> _allBattleDatas = new Dictionary<Type, IBattleData>();

        private readonly HashSet<Type> _allBattleDataTypes = new HashSet<Type>();


        private readonly Dictionary<string, long> lastNoticeTime = new Dictionary<string, long>();


        private bool ClearBattleData;

        public bool Stop
        {
            get => AEAssist.DataBinding.Instance.Stop;
            set => AEAssist.DataBinding.Instance.Stop = value;
        }

        public bool Move
        {
            get => AEAssist.DataBinding.Instance.Move;
            set => AEAssist.DataBinding.Instance.Move = value;
        }

        public bool CloseBurst
        {
            get => !AEAssist.DataBinding.Instance.Burst;
            set => AEAssist.DataBinding.Instance.Burst = !value;
        }

        public bool IsRunning => Core.Me.InCombat;

        public void Init()
        {
            _allBattleDatas.Clear();
            _allBattleDataTypes.Clear();
            var baseType = typeof(IBattleData);
            foreach (var type in GetType().Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                if (!baseType.IsAssignableFrom(type))
                    continue;
                _allBattleDataTypes.Add(type);
                _allBattleDatas[type] = Activator.CreateInstance(type) as IBattleData;
                LogHelper.Debug("Load BattleData: " + type);
            }
        }

        public static T GetBattleData<T>() where T : class, IBattleData
        {
            Instance._allBattleDatas.TryGetValue(typeof(T), out var data);
            return data as T;
        }

        public void Clear()
        {
            if (ClearBattleData)
                return;
            ForceClear();
        }

        public void ForceClear()
        {
            CountDownHandler.Instance.Close();

            _allBattleDatas.Clear();
            foreach (var type in _allBattleDataTypes)
                _allBattleDatas[type] = Activator.CreateInstance(type) as IBattleData;

            AEAssist.DataBinding.Instance.Reset();

            SpellHistoryMgr.Instance.Clear();
            AEGamelogManager.Instance.ClearAll();

            ClearBattleData = true;
            LogHelper.Debug("Clear battle data");
        }

        private bool _cleared;
        public async Task<bool> Update()
        {
            var timeNow = TimeHelper.Now();
            if (!ClearBattleData)
            {
                GetBattleData<BattleData>().Update(timeNow);
                SpellHistoryMgr.Instance.CheckIfNeedClearHistory();
            }

            AEAssist.DataBinding.Instance.Update();

            if (Stop)
            {
                if (!_cleared)
                {
                    _cleared = true;
                    Core.Me.ClearTarget();
                }

                GUIHelper.ShowInfo(Language.Instance.Content_AIRoot_Stoping);
                return false;
            }

            _cleared = false;
            var battleData = GetBattleData<BattleData>();
            if (battleData.NextAbilitySpellId != null)
            {
                if (SettingMgr.GetSetting<GeneralSettings>().NextAbilityFirst
                    || battleData.NextAbilitySpellId.IsHighPriority())
                {
                    var ret = battleData.NextAbilitySpellId;
                    if (ret.SpellData != null && ret.IsUnlock())
                    {
                        if (ret.CanCastAbility())
                        {
                            if (!await ret.DoAbility())
                            {
                                ret = null;
                                return false;
                            }

                            battleData.NextAbilitySpellId = null;
                        }
                        else
                        {
                            ret = null;
                        }
                    }
                    else
                    {
                        ret = null;
                        battleData.NextAbilitySpellId = null;
                    }

                    if (ret != null) RecordAbility(ret);
                }
            }

            if (!Core.Me.HasTarget || !Core.Me.CurrentTarget.CanAttack)
            {
                if (CanNotice("key1", 1000))
                    GUIHelper.ShowInfo(Language.Instance.Content_AIRoot_NoTarget, 500);

                await RotationManager.Instance.NoTarget();

                return false;
            }

            if (!((Character)Core.Me.CurrentTarget).HasTarget && !CountDownHandler.Instance.CanDoAction
                                                               && !AEAssist.DataBinding.Instance.Pull)
            {
                if (CanNotice("key2", 1000))
                    GUIHelper.ShowInfo(Language.Instance.Content_AIRoot_CanAttack, 500);
                return false;
            }


            if (Core.Me.InCombat)
            {
                if (ClearBattleData) battleData.BattleStartTime = timeNow;

                ClearBattleData = false;
            }

            if (battleData.NextAbilitySpellId != null && battleData.AbilityRetryEndTime < TimeHelper.Now())
            {
                LogHelper.Debug($"RetryEndTime : NextAbility {battleData.NextAbilitySpellId}");
                battleData.NextAbilitySpellId = null;
            }

            if (SettingMgr.GetSetting<GeneralSettings>().AutoInterrupt)
            {
                var battleCharacter = Core.Me.CurrentTarget as BattleCharacter;
                if (battleCharacter.IsCasting && battleCharacter.SpellCastInfo.Interruptible)
                {
                    var spell = SpellHelper.GetInterruptSpell(Core.Me.CurrentJob);
                    if (spell != 0 && spell.IsReady())
                    {
                        battleData.NextAbilitySpellId = spell.GetSpellEntity();
                    }
                }
            }

            if (await OpenerMgr.Instance.UseOpener(Core.Me.CurrentJob)) return false;


            if (await AISpellQueueMgr.Instance.UseSpellQueue()) return false;

            var spellQueue = AIRoot.GetBattleData<SpellQueueData>();
            if (await spellQueue.ApplySlot())
            {
                return false;
            }
            if (battleData.NextSpellSlot != null)
            {
                if (battleData.SlotRetryEndTime < TimeHelper.Now())
                {
                    battleData.NextSpellSlot = null;
                }


                if (battleData.NextSpellSlot != null)
                {
                    spellQueue.Add(battleData.NextSpellSlot);
                    battleData.NextSpellSlot = null;
                }

            }


            // boss is time to kill (default is 6s),so toggle the FinalBurst
            if (TTKHelper.CheckFinalBurst(Core.Me.CurrentTarget as Character))
                AEAssist.DataBinding.Instance.FinalBurst = true;

            var canUseAbility = true;
            var delta = timeNow - battleData.lastCastTime;
            var coolDown = GetGCDDuration();
            var time = SettingMgr.GetSetting<GeneralSettings>().RegionOfAbility;
            var RegionOfAbility = coolDown - time;
            if (RegionOfAbility > delta * 0.33f)
                RegionOfAbility = delta * 0.33f;
            var canUseGCD = CanUseGCD();
            LogHelper.Debug($"CanUseGCD: {canUseGCD} coolDown: {coolDown} delta {delta}");

            if (!canUseGCD && !Core.Me.IsCasting && battleData.maxAbilityTimes > 0 && coolDown - delta > RegionOfAbility)
                canUseAbility = true;
            else
                // LogHelper.Debug(
                //     $"NoAbility==> Need:{needDura} Times:{battleData.maxAbilityTimes} Delta: {coolDown - delta}");
                canUseAbility = false;

            if (canUseGCD)
            {
                SpellEntity ret = null;
                if (battleData.NextGcdSpellId != null)
                {
                    ret = battleData.NextGcdSpellId;
                    LogHelper.Debug($"NextGcd: {ret.Id} {ret.SpellData.LocalizedName}  {ret.IsUnlock()}");
                    if (ret.SpellData != null && ret.IsUnlock())
                    {
                        LogHelper.Debug($"NextGcd: {ret.Id} {ret.SpellData.LocalizedName} CanCast:  {ret.CanCastGCD()}");
                        if (ret.CanCastGCD() >= 0)
                        {
                            if (!await ret.DoGCD())
                                ret = null;
                            else
                                battleData.NextGcdSpellId = null;
                        }
                        else
                        {
                            ret = null;
                        }
                    }
                    else
                    {
                        ret = null;
                        battleData.NextGcdSpellId = null;
                    }

                    if (ret == null && battleData.GCDRetryEndTime < TimeHelper.Now())
                    {
                        LogHelper.Debug($"RetryEndTime : NextGCD {battleData.NextGcdSpellId?.Id}");
                        battleData.NextGcdSpellId = null;
                    }
                }

                if (ret == null)
                    ret = await AIMgrs.Instance.HandleGCD(Core.Me.CurrentJob, battleData.lastGCDSpell);
                if (ret != null) RecordGCD(ret);
            }

            if (canUseAbility)
            {
                SpellEntity ret = null;

                if (battleData.maxAbilityTimes == SettingMgr.GetSetting<GeneralSettings>().MaxAbilityTimsInGCD &&
                    battleData.NextAbilityUsePotion)
                {
                    battleData.NextAbilitySpellId = null;
                    var msg = "===>Try using Potion";
                    LogHelper.Info(msg);
                    var boolRet = await AIMgrs.Instance.UsePotion(Core.Me.CurrentJob);
                    if (boolRet)
                    {
                        battleData.NextAbilityUsePotion = false;
                        MuteAbilityTime();
                        return false;
                    }
                }
                else if (battleData.NextAbilitySpellId != null)
                {
                    ret = battleData.NextAbilitySpellId;
                    if (ret.SpellData != null && ret.IsUnlock())
                    {
                        if (ret.CanCastAbility())
                        {
                            if (!await ret.DoAbility())
                                ret = null;
                            else
                                battleData.NextAbilitySpellId = null;
                        }
                        else
                        {
                            ret = null;
                        }
                    }
                    else
                    {
                        ret = null;
                        battleData.NextAbilitySpellId = null;
                    }
                }

                if (ret == null)
                    ret = await AIMgrs.Instance.HandleAbility(Core.Me.CurrentJob, battleData.lastAbilitySpell);
                if (ret != null)
                    RecordAbility(ret);
                //LogHelper.Info($"剩余使用能力技能次数: {_maxAbilityTimes}");
            }

            return false;
        }

        public void RecordGCD(SpellEntity ret)
        {
            var battleData = GetBattleData<BattleData>();
            var timeNow = TimeHelper.Now();
            var msg = $"{battleData.CurrBattleTimeInMs / 1000} Cast GCD: {ret.Id} " + ret.SpellData.LocalizedName;
            LogHelper.Info(msg);
            GUIHelper.ShowInfo(msg, 100, false);
            var history = new SpellHistory
            {
                SpellId = ret.Id,
                CastTime = timeNow,
                Name = ret.SpellData.LocalizedName
            };
            SpellHistoryMgr.Instance.AddGCDHistory(history);


            if (battleData.lastGCDSpell == null)
                CountDownHandler.Instance.Close();
            battleData.lastGCDIndex++;
            battleData.lastGCDSpell = ret;
            battleData.lastCastTime = timeNow;
            battleData.ResetMaxAbilityTimes();
            battleData.lastAbilitySpell = null;
        }

        public void RecordAbility(SpellEntity ret)
        {
            var battleData = GetBattleData<BattleData>();
            var timeNow = TimeHelper.Now();
            var msg = $"{battleData.CurrBattleTimeInMs / 1000} Cast Ability: {ret.Id} " + ret.SpellData.LocalizedName;
            LogHelper.Info(msg);
            GUIHelper.ShowInfo(msg, 100, false);
            var history = new SpellHistory
            {
                SpellId = ret.Id,
                CastTime = timeNow,
                Name = ret.SpellData.LocalizedName,
                GCDIndex = battleData.lastGCDIndex
            };
            SpellHistoryMgr.Instance.AddAbilityHistory(history);
            battleData.maxAbilityTimes--;
            battleData.lastAbilitySpell = ret;
        }

        public bool Is2ndAbilityTime(float time = 0.5f)
        {
            if (SettingMgr.GetSetting<GeneralSettings>().MaxAbilityTimsInGCD < 2)
                return true;
            if (GetBattleData<BattleData>().maxAbilityTimes <
                SettingMgr.GetSetting<GeneralSettings>().MaxAbilityTimsInGCD)
                return true;

            var delta = TimeHelper.Now() - GetBattleData<BattleData>().lastCastTime;
            var coolDown = GetGCDDuration();

            if (coolDown - delta < coolDown * time)
                return true;
            return false;
        }

        public double GetGCDDuration()
        {
            var spell = RotationManager.Instance.GetBaseGCDSpell();
            return spell.AdjustedCooldown.TotalMilliseconds;
        }

        // gcd技能处于没有冷却完毕状态 (考虑了队列时间,实际上是是否可以发出指令使用GCD技能了)
        public bool CanUseGCD()
        {
            var ret = RotationManager.Instance.GetBaseGCDSpell();
            return ret.Cooldown.TotalMilliseconds < SettingMgr.GetSetting<GeneralSettings>().ActionQueueMs;
        }

        public void MuteAbilityTime()
        {
            GetBattleData<BattleData>().maxAbilityTimes = 0;
        }


        private bool CanNotice(string key, long interval)
        {
            var now = TimeHelper.Now();
            if (lastNoticeTime.TryGetValue(key, out var lastTime))
                if (lastTime + interval > now)
                    return false;

            lastNoticeTime[key] = now;
            return true;
        }
    }
}