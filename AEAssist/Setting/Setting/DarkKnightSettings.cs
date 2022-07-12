using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class DarkKnightSettings : IBaseSetting
    {
        public DarkKnightSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }

        public int ReserveManaPercentage { get; set; }
        public int FightorFlightTiming { get; set; }

        public int SheltronThreshold { get; set; }

        public bool DarkKnightDefenseMode { get; set; }//黑骑自动防御模式
        public void Reset()
        {
            ReserveManaPercentage = 40;
            FightorFlightTiming = 1;
            SheltronThreshold = 95;
            DarkKnightDefenseMode = true;
        }
        
        public bool IronWill { get; set; } = true;
        public bool Intervene { get; set; } = true;


        public void ResetToggles()
        {
            //Requiescat = true;
            DarkKnightDefenseMode = true;
        }

        public void OnLoad()
        {

        }
    }
}