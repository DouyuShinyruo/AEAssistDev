using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage
{
    public class WhiteMageSpellHelper
    {
        public static SpellEntity GetAero()
        {
            if (!SpellsDefine.Aero.IsUnlock())
            {
                LogHelper.Debug("Aero not unlocked. skipping.");
                return null;
            }

            if (!SpellsDefine.Aero2.IsUnlock())
            {
                LogHelper.Debug("Aero2 not unlocked trying to use Aero instead.");
                if (!ActionManager.HasSpell(SpellsDefine.Aero))
                {
                    LogHelper.Debug("Aero not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Aero found trying to use..");
                return SpellsDefine.Aero.GetSpellEntity();
            }

            if (!SpellsDefine.Dia.IsUnlock())
            {
                LogHelper.Debug("Dia not unlocked trying to use Aero2 instead.");
                if (!ActionManager.HasSpell(SpellsDefine.Aero2))
                {
                    LogHelper.Debug("Aero2 not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Aero2 found trying to use..");
                return SpellsDefine.Aero2.GetSpellEntity();
            }
            if (!ActionManager.HasSpell(SpellsDefine.Dia))
            {
                LogHelper.Debug("Dia not found. skipping.");
                return null;
            }

            LogHelper.Debug("Dia found trying to use..");
            return SpellsDefine.Dia.GetSpellEntity();
        }
        public static SpellEntity GetStone()
        {
            if (!SpellsDefine.Stone.IsUnlock())
            {
                LogHelper.Debug("Stone not unlocked. skipping.");
                return null;
            }
            if (!SpellsDefine.Stone2.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.Stone))
                {
                    LogHelper.Debug("Stone not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Using Stone. ");
                return SpellsDefine.Stone.GetSpellEntity();
            }
            if (!SpellsDefine.Stone3.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.Stone2))
                {
                    LogHelper.Debug("Stone2 not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Using Stone2. ");
                return SpellsDefine.Stone2.GetSpellEntity();
            }
            if (!SpellsDefine.Stone4.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.Stone3))
                {
                    LogHelper.Debug("Stone3 not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Using Stone3. ");
                return SpellsDefine.Stone3.GetSpellEntity();
            }
            if (!SpellsDefine.Glare.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.Stone4))
                {
                    LogHelper.Debug("Stone4 not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Using Stone4. ");
                return SpellsDefine.Stone4.GetSpellEntity();
            }
            if (!SpellsDefine.GlareIII.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.Glare))
                {
                    LogHelper.Debug("Glare not found. skipping.");
                    return null;
                }

                LogHelper.Debug("Using Glare. ");
                return SpellsDefine.Glare.GetSpellEntity();
            }
            if (ActionManager.HasSpell(SpellsDefine.GlareIII)) return SpellsDefine.GlareIII.GetSpellEntity();
            LogHelper.Debug("GlareIII not found: unlocked?");
            return null;
        }

        public static SpellEntity GetHoly()
        {
            if (!SpellsDefine.Holy.IsUnlock())
            {
                LogHelper.Debug("Holy not unlocked. skipping.");
                return null;
            }

            if (!SpellsDefine.HolyIII.IsUnlock())
            {
                if (!ActionManager.HasSpell(SpellsDefine.Holy))
                {
                    LogHelper.Debug("Holy not found: unlocked?");
                    return null;
                }

                LogHelper.Debug("Using Holy");
                return SpellsDefine.Holy.GetSpellEntity();
            }

            if (ActionManager.HasSpell(SpellsDefine.HolyIII)) return SpellsDefine.HolyIII.GetSpellEntity();
            LogHelper.Debug("HolyIII not found: unlocked?");
            return null;
        }
        public static SpellEntity GetBaseGcd()
        {
            return GetStone();
        }

        private static int GetAeroAura()
        {
            LogHelper.Debug("Checking if Aero is unlocked...");
            if (!SpellsDefine.Aero.IsUnlock())
            {
                LogHelper.Debug("Aero not unlocked...");
                return 0;
            }
            LogHelper.Debug("Checking if Aero2 is unlocked...");
            if (!SpellsDefine.Aero2.IsUnlock())
            {
                LogHelper.Debug("Aero2 not unlocked.. trying to use Aero instead.. ");
                if (!ActionManager.HasSpell(SpellsDefine.Aero))
                {
                    LogHelper.Debug("Failed to use Aero...");
                    return 0;
                }

                LogHelper.Debug("Aero found using...");
                return AurasDefine.Aero;
            }
            LogHelper.Debug("Checking if Dia is unlocked...");
            if (!SpellsDefine.Dia.IsUnlock())
            {
                LogHelper.Debug("Dia not unlocked.. trying to use Aero2 instead.. ");
                if (!ActionManager.HasSpell(SpellsDefine.Aero2))
                {
                    LogHelper.Debug("Failed to use Aero2...");
                    return 0;
                }

                LogHelper.Debug("Aero2 found using...");
                return AurasDefine.Aero2;
            }
            LogHelper.Debug("Checking if Dia is unlocked...");
            if (SpellsDefine.Dia.IsUnlock())
            {
                LogHelper.Debug("Using Dia");
                return AurasDefine.Dia;
            }
            return AurasDefine.Dia;
        }

        public static bool IsTargetHasAuraAero(Character target)
        {
            var id = GetAeroAura();
            LogHelper.Debug("Checking if target has Areo: " + target.EnglishName);
            return id == 0 || target.HasMyAuraWithTimeleft((uint)id);
        }
        public static void RecordAero()
        {
            var targetId = Core.Me.CurrentTarget.ObjectId;
            AIRoot.GetBattleData<WhiteMageBattleData>().lastAeroWithObj[targetId] = true;
        }
        public static void RemoveRecordAero()
        {
            var targetId = Core.Me.CurrentTarget.ObjectId;
            AIRoot.GetBattleData<WhiteMageBattleData>().lastAeroWithObj[targetId] = false;
        }
        public static bool IsTargetNeedAero(Character target, int timeLeft)
        {
            var aeroId = GetAeroAura();
            LogHelper.Debug("Checking if target need Aero id: " + aeroId);
            if (aeroId == 0) return false;

            var ttkAero = SettingMgr.GetSetting<WhiteMageSettings>().TTK_Aero;
            if (Core.Me.ClassLevel < 72)
            {
                ttkAero = 18;
            }
            bool NormalCheck()
            {
                if (DataBinding.Instance.EarlyDecisionMode)
                    timeLeft += SettingMgr.GetSetting<GeneralSettings>().ActionQueueMs;
                return !target.HasMyAuraWithTimeleft((uint)aeroId, timeLeft);
            }

            if (AIRoot.GetBattleData<WhiteMageBattleData>().IsTargetLastAero()) return NormalCheck();
            if (ttkAero > 0 && target.HasMyAuraWithTimeleft((uint)aeroId, ttkAero * 1000) &&
                TTKHelper.IsTargetTTK(target, ttkAero, false))
                return NormalCheck();

            return NormalCheck();
        }
        public static async Task CastSwiftCast()
        {
            LogHelper.Debug("Checking if we have swiftcast aura or Swiftcast recentlyUsed");
            if (Core.Me.HasAura(AurasDefine.Swiftcast) || SpellsDefine.Swiftcast.RecentlyUsed()) return;
            LogHelper.Debug("Swiftcast can be used: using.");
            var spell = SpellsDefine.Swiftcast.GetSpellEntity();
            var ret = await spell.DoAbility();
        }
        public static async Task CastThinAir()
        {
            if (Core.Me.HasAura(AurasDefine.ThinAir) || SpellsDefine.ThinAir.RecentlyUsed()) return;
            var spell = SpellsDefine.ThinAir.GetSpellEntity();
            var ret = await spell.DoAbility();
        }
        public static async Task CastRaiseToTarget(Character target)
        {
            if (!SpellsDefine.Raise.IsUnlock()) return;
            await CastSwiftCast();
            await CastThinAir();
            var spell = new SpellEntity(SpellsDefine.Raise, target as BattleCharacter);
            await spell.DoGCD();
        }

        public static async Task<SpellEntity> CastResPriority()
        {
            var priority = SettingMgr.GetSetting<WhiteMageSettings>().WhiteMageResPriority;
            var deadAllies = GroupHelper.DeadAllies;

            switch (priority)
            {
                // Healer>Tanks>DPS
                case 0:
                    LogHelper.Debug("Healer>Tanks>DPS-RESSING");
                    foreach (var deadAlly in deadAllies)
                    {
                        // check if the player already ressed.
                        LogHelper.Debug("checking if the player already rezzed if so skipping.");
                        if (deadAlly.HasAura(AurasDefine.Raise)) continue;

                        // check if the distance from the player is more than 30
                        if (deadAlly.Distance(Core.Me) >= 40) continue;

                        if (deadAlly.IsDps())
                        {
                            // check if there is tank that's dead too.
                            if (deadAllies.Any(deadTanks => deadTanks.IsTank()))
                            {
                                if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                                {
                                    LogHelper.Debug("Trying to swift res the healer.");
                                    await CastRaiseToTarget(deadAlly);
                                    return null;
                                }
                                LogHelper.Debug("Trying to swift res the tank.");
                                await CastRaiseToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the dps.");
                            await CastRaiseToTarget(deadAlly);
                            return null;
                        }

                        if (deadAlly.IsTank())
                        {
                            // check if there is healers that are dead too.
                            if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                            {
                                LogHelper.Debug("Trying to swift res the healer.");
                                await CastRaiseToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the tank.");
                            await CastRaiseToTarget(deadAlly);
                            return null;
                        }

                        if (!deadAlly.IsHealer()) continue;
                        LogHelper.Debug("Trying to swift res the healer.");
                        await CastRaiseToTarget(deadAlly);
                        return null;
                    }
                    return null;
                // Tanks>Healer>DPS
                case 1:
                    LogHelper.Debug("Tanks>Healer>DPS-RESSING");
                    foreach (var deadAlly in deadAllies)
                    {
                        if (deadAlly.IsDps())
                        {
                            // check if there is healers that are dead too.
                            if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                            {
                                if (deadAllies.Any(deadTanks => deadTanks.IsTank()))
                                {
                                    LogHelper.Debug("Trying to swift res the tank.");
                                    await CastRaiseToTarget(deadAlly);
                                    return null;
                                }
                                LogHelper.Debug("Trying to swift res the healer.");
                                await CastRaiseToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the dps.");
                            await CastRaiseToTarget(deadAlly);
                            return null;
                        }

                        if (deadAlly.IsHealer())
                        {
                            // check if there is healers that are dead too.
                            if (deadAllies.Any(deadTanks => deadTanks.IsTank()))
                            {
                                LogHelper.Debug("Trying to swift res the tank.");
                                await CastRaiseToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the healer.");
                            await CastRaiseToTarget(deadAlly);
                            return null;
                        }

                        if (!deadAlly.IsTank()) continue;
                        LogHelper.Debug("Trying to swift res the tank.");
                        await CastRaiseToTarget(deadAlly);
                        return null;
                    }
                    return null;
                // DPS>Healer>Tanks
                case 2:
                    foreach (var deadAlly in deadAllies)
                    {
                        // check if the player already ressed.
                        LogHelper.Debug("checking if the player already rezzed if so skipping.");
                        if (deadAlly.HasAura(AurasDefine.Raise)) continue;

                        // check if the distance from the player is more than 30
                        if (deadAlly.Distance(Core.Me) >= 40) continue;

                        if (deadAlly.IsTank())
                        {
                            // check if there is dps that's dead too.
                            if (deadAllies.Any(deadHealer => deadHealer.IsHealer()))
                            {
                                if (deadAllies.Any(deadDps => deadDps.IsDps()))
                                {
                                    LogHelper.Debug("Trying to swift res the dps.");
                                    await CastRaiseToTarget(deadAlly);
                                    return null;
                                }
                                LogHelper.Debug("Trying to swift res the healer.");
                                await CastRaiseToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the tanks.");
                            await CastRaiseToTarget(deadAlly);
                            return null;
                        }

                        if (deadAlly.IsHealer())
                        {
                            // check if there is dps that are dead too.
                            if (deadAllies.Any(deadDps => deadDps.IsDps()))
                            {
                                LogHelper.Debug("Trying to swift res the DPS.");
                                await CastRaiseToTarget(deadAlly);
                                return null;
                            }
                            LogHelper.Debug("Trying to swift res the healer.");
                            await CastRaiseToTarget(deadAlly);
                            return null;
                        }

                        if (!deadAlly.IsDps()) continue;
                        LogHelper.Debug("Trying to swift res the DPS.");
                        await CastRaiseToTarget(deadAlly);
                        return null;
                    }
                    return null;
            }
            return null;
        }

        /*public static async Task<SpellEntity> CastTetragrammaton(Character target)
        {
            if (!SpellsDefine.Tetragrammaton.IsUnlock()) return;
            
            if (GroupHelper.InParty)
            {
                var tetragrammatonTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().TetragrammatonHp);
                if(tetragrammatonTarget == null)
                {
                    return false;
                }
                //var spell = new SpellEntity(SpellsDefine.Tetragrammaton, tetragrammatonTarget as BattleCharacter);
                //return await spell.DoAbility();
                return true;
            }
            return false;
        }*/
        public static async Task<SpellEntity> CastTetragrammaton()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().TetragrammatonHp);

                if (!SpellsDefine.Tetragrammaton.IsUnlock()) return null;
                var spell = new SpellEntity(SpellsDefine.Tetragrammaton, skillTarget as BattleCharacter);
                await spell.DoAbility();
                //await CastTetragrammaton(skillTarget);
            }
            return null;
        }

        public static async Task<SpellEntity> CastDivineBenison()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().DivineBenisonHp);
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.DivineBenison.IsUnlock()) return null;
                var spell = new SpellEntity(SpellsDefine.DivineBenison, skillTarget as BattleCharacter);
                await spell.DoAbility();
            }
            return null;

        }
        public static async Task<SpellEntity> CastRegen()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().RegenHp && !r.HasAura(AurasDefine.Regen));
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.Regen.IsUnlock()) return null;
                var spell = new SpellEntity(SpellsDefine.Regen, skillTarget as BattleCharacter);
                LogHelper.Debug("再生释放目标：" + Convert.ToString(skillTarget));
                await spell.DoGCD();
            }
            return null;

        }
        public static async Task<SpellEntity> CastAfflatusSolace()
        {

            if (GroupHelper.InParty)
            {
                var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().AfflatusSolaceHp);
                //await CastDivineBenison(skillTarget);
                if (!SpellsDefine.AfflatusSolace.IsUnlock()) return null;
                var spell = new SpellEntity(SpellsDefine.AfflatusSolace, skillTarget as BattleCharacter);
                LogHelper.Debug("安慰之心释放目标：" + Convert.ToString(skillTarget));
                await spell.DoGCD();
            }
            return null;

        }
    }
}
