using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class PaladinSettings : IBaseSetting
    {
        public PaladinSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }

        public int ReserveManaPercentage { get; set; }
        public int FightorFlightTiming { get; set; }

        public int SheltronThreshold { get; set; }
        public void Reset()
        {
            ReserveManaPercentage = 40;
            FightorFlightTiming = 1;
            SheltronThreshold = 95;
        }

        public bool Requiescat { get; set; } = true;
        public bool IronWill { get; set; } = true;

        public bool Intervene { get; set; } = true;

        public bool Sheltron { get; set; } = true;
        public void ResetToggles()
        {
            Requiescat = true;
            IronWill = true;
            Intervene = true;
            Sheltron = true;
        }

        public void OnLoad()
        {

        }
    }
}