using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_Songs : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!AEAssist.DataBinding.Instance.UseSong)
                return -10;
            if (TimeHelper.Now() - AIRoot.GetBattleData<BardBattleData>().lastCastSongTime < 3000)
                return -2;

            if (!SpellsDefine.TheWanderersMinuet.IsReady()
                && !SpellsDefine.MagesBallad.IsReady()
                && !SpellsDefine.ArmysPaeon.IsReady())
                return -4;

            var spell = CheckNeedChangeSong(out var forceNextSong);
            if (spell == null)
                return -5;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = CheckNeedChangeSong(out var forceNextSong);
            if (spell == null)
                return null;

            var castPitch = false;

            if (ActionResourceManager.Bard.ActiveSong == ActionResourceManager.Bard.BardSong.WanderersMinuet)
                if (SpellsDefine.PitchPerfect.IsReady())
                    if (ActionResourceManager.Bard.Repertoire > 0 && await SpellsDefine.PitchPerfect.DoAbility())
                        castPitch = true;

            if (AIRoot.GetBattleData<BattleData>().CurrBattleTimeInMs > 10000 && spell.Id == SpellsDefine.TheWanderersMinuet)
            {
                var halfGCD = AIRoot.Instance.GetGCDDuration() / 2;
                await Coroutine.Sleep((int)(halfGCD - 100));
            }

            var ret = await spell.DoAbility();
            if (ret)
            {
                var battleData = AIRoot.GetBattleData<BardBattleData>();
                if (forceNextSong) battleData.RemoveFirstInNextSong();

                if (castPitch) AIRoot.Instance.MuteAbilityTime();

                return spell;
            }

            return null;
        }


        private SpellEntity CheckNeedChangeSong(out bool forceNextSong)
        {
            var bardBattleData = AIRoot.GetBattleData<BardBattleData>();
            var currSong = ActionResourceManager.Bard.ActiveSong;
            if (currSong == ActionResourceManager.Bard.BardSong.None)
                currSong = bardBattleData.lastSong;
            // 300是因为考虑到服务器延时
            var remainTime = ActionResourceManager.Bard.Timer.TotalMilliseconds - 300;
            SpellEntity spell = null;
            forceNextSong = false;
            if (AIRoot.Instance.CloseBurst)
            {
                if (currSong != ActionResourceManager.Bard.BardSong.None)
                {
                    if (bardBattleData.ControlByNextSongQueue((int)currSong))
                    {
                        if (remainTime <= 45000 - bardBattleData.nextSongDuration[0])
                            spell = GetSongsByOrder(null, out forceNextSong);

                        return spell;
                    }

                    if (remainTime <= ConstValue.SongsTimeLeftCheckWhenCloseBuff)
                    {
                        spell = GetSongsByOrder(null, out forceNextSong);
                        return spell;
                    }

                    return null;
                }

                spell = GetSongsByOrder(null, out forceNextSong);
            }
            else
            {
                if (bardBattleData.ControlByNextSongQueue((int)currSong))
                {
                    if (remainTime <= 45000 - bardBattleData.nextSongDuration[0])
                        spell = GetSongsByOrder(null, out forceNextSong);
                }
                else
                {
                    switch (currSong)
                    {
                        case ActionResourceManager.Bard.BardSong.None:
                            spell = GetSongsByOrder(null, out forceNextSong);
                            break;
                        case ActionResourceManager.Bard.BardSong.WanderersMinuet:
                            if (remainTime <= SettingMgr.GetSetting<BardSettings>().Songs_WM_TimeLeftForSwitch)
                                spell = GetSongsByOrder(SpellsDefine.TheWanderersMinuet.GetSpellEntity(),
                                    out forceNextSong);

                            break;
                        case ActionResourceManager.Bard.BardSong.MagesBallad:
                            if (SpellsDefine.TheWanderersMinuet.GetSpellEntity().Cooldown.TotalMilliseconds < 100
                                 || remainTime <= SettingMgr.GetSetting<BardSettings>().Songs_MB_TimeLeftForSwitch)
                                spell = GetSongsByOrder(SpellsDefine.MagesBallad.GetSpellEntity(), out forceNextSong);

                            break;
                        case ActionResourceManager.Bard.BardSong.ArmysPaeon:
                            if (SpellsDefine.TheWanderersMinuet.GetSpellEntity().Cooldown.TotalMilliseconds < 100
                                || remainTime <= SettingMgr.GetSetting<BardSettings>().Songs_AP_TimeLeftForSwitch)
                                spell = GetSongsByOrder(SpellsDefine.ArmysPaeon.GetSpellEntity(), out forceNextSong);

                            break;
                    }
                }
            }

            if (spell != null)
                GUIHelper.ShowInfo($"Song: {spell.SpellData.LocalizedName} remainTime: {remainTime}", 1000, false);

            return spell;
        }

        private SpellEntity GetSongsByOrder(SpellEntity passSpell, out bool forceNextSong)
        {
            SpellEntity spell = null;
            forceNextSong = false;
            var bardBattleData = AIRoot.GetBattleData<BardBattleData>();
            var currSong = ActionResourceManager.Bard.ActiveSong;
            if (currSong == ActionResourceManager.Bard.BardSong.None)
                currSong = bardBattleData.lastSong;
            if (bardBattleData.ControlByNextSongQueue((int)currSong))
            {
                var nextSong = AIRoot.GetBattleData<BardBattleData>().GetNextSong();
                switch ((ActionResourceManager.Bard.BardSong)nextSong)
                {
                    case ActionResourceManager.Bard.BardSong.MagesBallad:
                        spell = SpellsDefine.MagesBallad.GetSpellEntity();
                        break;
                    case ActionResourceManager.Bard.BardSong.ArmysPaeon:
                        spell = SpellsDefine.ArmysPaeon.GetSpellEntity();
                        break;
                    case ActionResourceManager.Bard.BardSong.WanderersMinuet:
                        spell = SpellsDefine.TheWanderersMinuet.GetSpellEntity();
                        break;
                    case ActionResourceManager.Bard.BardSong.None:
                        forceNextSong = true;
                        break;
                }

                if (spell != null && spell.IsReady())
                {
                    forceNextSong = true;
                    return spell;
                }
            }

            spell = NextSong(passSpell);
            if (spell != null && spell.IsReady()) return spell;

            spell = NextSong(spell);
            if (spell != null && spell.IsReady())
                return spell;
            return null;
        }

        private SpellEntity NextSong(SpellEntity SpellEntity)
        {
            var i = BardSpellHelper.Songs.Count;
            var origin = SpellEntity;
            while (i > 0)
            {
                SpellEntity = BardSpellHelper.Songs.GetNext(SpellEntity);
                i--;
                if (SpellEntity == null || !SpellEntity.IsReady()) continue;

                if (SpellEntity.Id == SpellsDefine.TheWanderersMinuet && AIRoot.Instance.CloseBurst) continue;

                if (SpellEntity == origin)
                    continue;
                return SpellEntity;
            }

            return null;
        }
    }
}