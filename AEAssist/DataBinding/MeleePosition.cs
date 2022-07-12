using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using PropertyChanged;
using System;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class MeleePosition
    {
        public static MeleePosition Intance = new MeleePosition();

        public enum Position
        {
            Back,
            Side,
            None
        }

        public enum Priority
        {
            Low,
            Medium,
            High
        }

        private Position CurrentPosition { get; set; } = Position.None;
        private Position RequiredPosition { get; set; } = Position.None;
        public Priority CurrentPriority { get; set; } = Priority.Low;
        public String RequiredPositionString { get; set; } = "";
        public bool IsPositionCorrect { get; set; } = true;

        public int TargetDistance { get; set; } = 0;

        public System.Windows.Media.Brush IsPositionCorrectColor { get; set; } = System.Windows.Media.Brushes.Green;
        public System.Windows.Media.Brush IsTargetDistanceSafe { get; set; } = System.Windows.Media.Brushes.Green;

        private void SetString()
        {
            switch (RequiredPosition)
            {
                case Position.Back:
                    RequiredPositionString = "▲▲▲";
                    break;
                case Position.Side:
                    RequiredPositionString = "▶◎◀";
                    break;
                default:
                    RequiredPositionString = "◎";
                    break;
            }
            // UIHelper.RfreshCurrOverlay();
        }

        public void SetPositionToBack(Priority priority = Priority.Low)
        {
            RequiredPosition = Position.Back;
            CurrentPriority = priority;
        }

        public void SetPositionToSide(Priority priority = Priority.Low)
        {
            RequiredPosition = Position.Side;
            CurrentPriority = priority;
        }

        public void SetPositionToNone(Priority priority = Priority.Low)
        {
            RequiredPosition = Position.None;
            CurrentPriority = priority;
        }

        public Priority GetPriority()
        {
            // LogHelper.Debug("Current Priority is: " + CurrentPriority);
            return CurrentPriority;
        }

        public Position GetRequiredPosition()
        {
            return RequiredPosition;
        }


        private void SetColor()
        {
            switch (IsPositionCorrect)
            {
                case false:
                    if (CurrentPriority == Priority.High)
                    {
                        IsPositionCorrectColor = System.Windows.Media.Brushes.DarkRed;
                    }
                    else if (CurrentPriority == Priority.Medium)
                    {
                        IsPositionCorrectColor = System.Windows.Media.Brushes.DarkGoldenrod;
                    }
                    else
                    {
                        IsPositionCorrectColor = System.Windows.Media.Brushes.DarkKhaki;
                    }
                    break;
                default:
                    IsPositionCorrectColor = System.Windows.Media.Brushes.Green;
                    break;
            }
        }

        private void SetDistance(GameObject CurrentTarget)
        {
            var distanceF = TargetHelper.GetTargetDistanceFromMeTest(CurrentTarget, Core.Me);
            var distanceI = (int)Math.Round(distanceF * 100);
            TargetDistance = distanceI;
            if (distanceI > 300)
            {
                TargetDistance = 300;
            }

            if (distanceI > 300)
            {
                IsTargetDistanceSafe = System.Windows.Media.Brushes.DarkRed;
            }
            else if (200 < distanceI)
            {
                IsTargetDistanceSafe = System.Windows.Media.Brushes.Coral;
            }
            else
            {
                IsTargetDistanceSafe = System.Windows.Media.Brushes.Green;
            }


        }

        public void ShowMsg()
        {
            if (Core.Me.HasTarget)
            {
                var CurrentTarget = Core.Me.CurrentTarget;

                SetDistance(CurrentTarget);
                if (CurrentTarget.IsBehind)
                {
                    CurrentPosition = Position.Back;
                }
                else if (CurrentTarget.IsFlanking)
                {
                    CurrentPosition = Position.Side;
                }
                if (RequiredPosition != Position.None && CurrentPosition != RequiredPosition)
                {
                    IsPositionCorrect = false;
                }
                else
                {
                    IsPositionCorrect = true;
                }
                SetString();
                SetColor();
            }
            // if (!check)
            //     UIHelper.RfreshCurrOverlay();
        }
    }
}