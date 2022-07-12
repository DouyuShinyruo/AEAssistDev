using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class WarriorSettings : IBaseSetting
    {
        public WarriorSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }

        public int ReserveManaPercentage { get; set; }
        public int FightorFlightTiming { get; set; }

        public int SheltronThreshold { get; set; }

        public bool WarriorDefenseMode { get; set; }//战士自动防御模式
        public void Reset()
        {
            ReserveManaPercentage = 40;
            FightorFlightTiming = 1;
            SheltronThreshold = 95;
            WarriorDefenseMode = true;
        }
        
        public bool IronWill { get; set; } = true;
        public bool Intervene { get; set; } = true;

        public bool WarriorRampart { get; set; } = true;//铁壁
        public bool WarriorBloodwhetting { get; set; } = true;//原初的血气
        public bool WarriorVengeance { get; set; } = true;//复仇

        public void ResetToggles()
        {
            //Requiescat = true;
            WarriorDefenseMode = true;
            WarriorRampart = true;
            WarriorVengeance = true;
            WarriorBloodwhetting = true;
        }

        public void OnLoad()
        {

        }
    }
}