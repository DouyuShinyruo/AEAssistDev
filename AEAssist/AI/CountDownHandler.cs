using AEAssist.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEAssist.AI
{
    public class CountDownHandler
    {
        public static CountDownHandler Instance = new CountDownHandler();
        private long _lastTime;
        private long CountDown = 5000;

        private readonly Dictionary<long, Func<Task>> CountDownActions = new Dictionary<long, Func<Task>>();

        private readonly HashSet<long> HasDone = new HashSet<long>();

        public bool Start { get; private set; }

        public bool CanDoAction { get; private set; }

        public async Task Update()
        {
            if (!Start)
                return;
            var restTime = CountDown - (TimeHelper.Now() - _lastTime);
            var msg = $"{Language.Instance.Content_CoolDown} {restTime} ms";


            foreach (var v in CountDownActions)
            {
                if (HasDone.Contains(v.Key))
                    continue;
                if (restTime <= v.Key)
                {
                    HasDone.Add(v.Key);
                    try
                    {
                        LogHelper.Info("DoCountDownAction: " + v.Key);
                        await v.Value.Invoke();
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e.ToString());
                    }
                }
            }

            if (restTime < 100)
            {
                msg = $"{Language.Instance.Content_CoolDownFinish}";
                CanDoAction = true;
                Start = false;
            }

            GUIHelper.ShowInfo(msg, 100, false);
        }

        public void StartCountDown()
        {
            _lastTime = TimeHelper.Now();
            CanDoAction = false;
            Start = true;
            HasDone.Clear();
        }

        public void SyncRestTime(int restTime)
        {
            if (!Start)
            {
                CountDown = restTime * 1000;
                StartCountDown();
            }
        }

        public void Close()
        {
            Start = false;
            CanDoAction = false;
            HasDone.Clear();
        }

        public void StartNow()
        {
            Start = false;
            CanDoAction = true;
            HasDone.Clear();
        }

        public void ClearListener()
        {
            CountDownActions.Clear();
        }

        public void AddListener(int timeLeft, Func<Task> action)
        {
            CountDownActions[timeLeft] = action;
        }
    }
}