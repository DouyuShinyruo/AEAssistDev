﻿using System;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI
{
    public class BardAbility_Songs : IAIHandler
    {
        public bool Check(SpellData lastSpell)
        {
            var target = Core.Me.CurrentTarget as Character;
            if (TTKHelper.IsTargetTTK(target))
                return false;
            if (lastSpell == Spells.TheWanderersMinuet || lastSpell == Spells.MagesBallad || lastSpell == Spells.ArmysPaeon)
                return false;

            if (!Spells.TheWanderersMinuet.IsReady()
                && !Spells.MagesBallad.IsReady()
                && !Spells.ArmysPaeon.IsReady())
                return false;
            
            var currSong = ActionResourceManager.Bard.ActiveSong;
            var remainTime = ActionResourceManager.Bard.Timer.TotalMilliseconds;
            switch (currSong)
            {
                case ActionResourceManager.Bard.BardSong.None:
                    return true;
                case ActionResourceManager.Bard.BardSong.MagesBallad:
                    if (remainTime <= BardSettings.Instance.Songs_MB_TimeLeftForSwitch)
                        return true;
                    break;
                case ActionResourceManager.Bard.BardSong.ArmysPaeon:
                    if (remainTime <= BardSettings.Instance.Songs_AP_TimeLeftForSwitch)
                        return true;
                    break;
                case ActionResourceManager.Bard.BardSong.WanderersMinuet:
                    if (remainTime <= BardSettings.Instance.Songs_WM_TimeLeftForSwitch)
                        return true;
                    break;
            }

            return false;
        }

        public async Task<SpellData> Run()
        {
            var currSong = ActionResourceManager.Bard.ActiveSong;
            var remainTime = ActionResourceManager.Bard.Timer.TotalMilliseconds;
            SpellData spell = null;
            switch (currSong)
            {
                case ActionResourceManager.Bard.BardSong.None:
                    spell = GetSongsByOrder(null);
                    break;
                case ActionResourceManager.Bard.BardSong.MagesBallad:
                    if (remainTime <= BardSettings.Instance.Songs_MB_TimeLeftForSwitch)
                    {
                        spell = GetSongsByOrder(Spells.MagesBallad);
                    }
                    break;
                case ActionResourceManager.Bard.BardSong.ArmysPaeon:
                    if (remainTime <= BardSettings.Instance.Songs_AP_TimeLeftForSwitch)
                    {
                        spell = GetSongsByOrder(Spells.ArmysPaeon);
                    }

                    break;
                case ActionResourceManager.Bard.BardSong.WanderersMinuet:
                    if (remainTime <= BardSettings.Instance.Songs_WM_TimeLeftForSwitch)
                    {
                        spell = GetSongsByOrder(Spells.TheWanderersMinuet);
                        if (Spells.PitchPerfect.IsReady())
                        {
                            await SpellHelper.CastAbility(Spells.PitchPerfect, Core.Me.CurrentTarget, 100);
                        }
                    }
                    break;
            }

            if (spell == null)
                return null;
            var ret = await SpellHelper.CastAbility(spell, Core.Me.CurrentTarget);
            if (ret)
                return spell;
            return null;
        }

        private SpellData GetSongsByOrder(SpellData passSpell)
        {
            SpellData spell = null;
            if (passSpell != Spells.TheWanderersMinuet && !AIRoot.Instance.CloseBuff && Spells.TheWanderersMinuet.IsReady())
            {
                spell = Spells.TheWanderersMinuet;
                return spell;
            }
            if (passSpell != Spells.MagesBallad &&Spells.MagesBallad.IsReady())
            {
                spell = Spells.MagesBallad;
                return spell;
            }
            if (passSpell != Spells.ArmysPaeon && Spells.ArmysPaeon.IsReady())
            {
                spell = Spells.ArmysPaeon;
                return spell;
            }
            return spell;
        }
    }
}