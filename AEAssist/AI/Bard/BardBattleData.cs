using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Collections.Generic;

namespace AEAssist.AI.Bard
{
    public class BardBattleData : IBattleData
    {
        public long lastCastSongTime;
        public Dictionary<uint, bool> lastIronJawWithBuffWithObj = new Dictionary<uint, bool>();
        public ActionResourceManager.Bard.BardSong lastSong;

        public bool IsTargetLastIronJawWithBuff()
        {
            var targetId = Core.Me.CurrentTarget.ObjectId;
            lastIronJawWithBuffWithObj.TryGetValue(targetId, out var ret);
            return ret;
        }

        #region NextSongs

        public List<int> nextSongList = new List<int>();
        public List<int> nextSongDuration = new List<int>();


        public bool ControlByNextSongQueue(int currSong)
        {
            //  LogHelper.Debug("Check NextSongQueue: " + nextSongList.Count);
            if (nextSongList.Count == 0)
                return false;

            var peekSong = nextSongList[0];
            LogHelper.Debug($"Check NextSongQueue: Curr {currSong} Peek {peekSong}");
            if (currSong != peekSong) return false;

            return true;
        }

        public int GetNextSong()
        {
            if (nextSongList.Count <= 1)
                return 0;

            return nextSongList[1];
        }

        public bool NeedSwitchByNextSongQueue(int currSong, double remainTime)
        {
            if (nextSongList.Count == 0)
                return false;

            var peekSong = nextSongList[0];
            var peekDura = nextSongDuration[0];

            if (currSong != peekSong) return false;

            if (remainTime > 45000 - peekDura) return false;

            return true;
        }

        public void RemoveFirstInNextSong()
        {
            if (nextSongList.Count == 0)
                return;
            LogHelper.Info($"Remove NextSong : Index{nextSongList[0]} Dura{nextSongDuration[0]}");
            nextSongList.RemoveAt(0);
            nextSongDuration.RemoveAt(0);
        }

        #endregion
    }
}