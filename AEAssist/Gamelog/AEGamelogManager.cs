using AEAssist.AI;
using AEAssist.Helper;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AEAssist.Gamelog
{
    public class AEGamelogManager
    {
        public static AEGamelogManager Instance = new AEGamelogManager();

        public readonly QuickGraph.Collections.Queue<GameLog> GameLogBuffers =
            new QuickGraph.Collections.Queue<GameLog>();

        public Dictionary<int, string> Index2Characters = new Dictionary<int, string>();

        // private HashSet<int> MuteType = new HashSet<int>()
        // {
        //     4139,4400,4777,4905,4398,10283,
        // }

        public void Init()
        {
            GamelogManager.MessageRecevied += MessageRecevied;
        }

        public void CheckLog()
        {
            while (GameLogBuffers.Count >= 100)
            {
                var deq = GameLogBuffers.Dequeue();
                LogHelper.Info("RemoveGameLog=> " + deq.Content);
            }
        }

        public void ClearAll()
        {
            GameLogBuffers.Clear();
            LogHelper.Info("ClearAllGameLog");
        }

        private void AddBuffers(int msgType, string content)
        {
            if (msgType >= 10 && msgType < (int)MessageType.Echo)
                return;
            if (msgType > 1000)
                return;
            GameLogBuffers.Enqueue(new GameLog
            {
                MsgType = msgType,
                Content = content
            });
            if (SettingMgr.GetSetting<GeneralSettings>().ShowGameLog)
                LogHelper.Info($"GameLog==> MessageType:{msgType.ToString()} Content:{content}");
        }


        private HashSet<int> CheckTypes = new HashSet<int>()
        {
            185, 313, 569
        };



        private void MessageRecevied(object sender, ChatEventArgs e)
        {
            try
            {
                if (e.ChatLogEntry.MessageType == MessageType.Echo && e.ChatLogEntry.Contents.Contains("EAssistGetParty"))
                {
                    LogHelper.Info($"{e.ChatLogEntry.MessageType} " + e.ChatLogEntry.Contents);
                    var names = e.ChatLogEntry.Contents.Split(':')[1].Split('|');
                    for (int i = 0; i < names.Length; i++)
                    {
                        LogHelper.Info($"Index: {i} Name: {names[i]}");
                        Index2Characters[i] = names[i];
                    }
                    _action?.Invoke();
                }

                AddBuffers((int)e.ChatLogEntry.MessageType, e.ChatLogEntry.Contents);
                var type = (ushort)e.ChatLogEntry.MessageType;

                if (CheckTypes.Contains(type))
                {
                    if (e.ChatLogEntry.Contents.Contains(Language.Instance.MessageLog_CountDown_BattleStart))
                    {
                        var match = Regex.Match(e.ChatLogEntry.Contents,
                            Language.Instance.MessageLog_CountDown_BattleStartInTime);

                        if (match.Success && int.TryParse(match.Value, out var restTime))
                        {
                            LogHelper.Info("StartCountDown: " + e.ChatLogEntry.Contents);
                            CountDownHandler.Instance.SyncRestTime(restTime);
                        }

                        GUIHelper.ShowInfo(e.ChatLogEntry.Contents, 1000, false);
                    }

                    // someone cancel
                    if (e.ChatLogEntry.Contents.Contains(Language.Instance.MessageLog_CountDown_CancelBattleStart))
                    {
                        LogHelper.Info("CountDownEnd: " + e.ChatLogEntry.Contents);
                        CountDownHandler.Instance.Close();
                        GUIHelper.ShowInfo(e.ChatLogEntry.Contents, 1000, false);
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
            }
        }

        public void Close()
        {
            GamelogManager.MessageRecevied -= MessageRecevied;
        }

        public struct GameLog
        {
            public int MsgType;
            public string Content;
        }

        private Action _action;
        public void SayPartyLog(Action action)
        {
            var str = "/e AEAssistGetParty:<1>|<2>|<3>|<4>|<5>|<6>|<7>|<8>";
            ChatManager.SendChat(str);
            _action = action;
        }

        public GameObject GetPartyMemberByIndex(int index)
        {
            this.Index2Characters.TryGetValue(index, out var character);
            LogHelper.Info($"GetIndex: {index} {character} {GameObjectManager.GameObjects?.Count()}");
            if (!string.IsNullOrEmpty(character))
            {
                var gameObj = GameObjectManager.GameObjects.FirstOrDefault(v =>
                {
                    return v.Name == character;
                });
                return gameObj;
            }

            return null;
        }
    }
}