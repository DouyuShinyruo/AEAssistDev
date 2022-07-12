using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class SMNSettings : IBaseSetting
    {
        public SMNSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }
        //

        public int SwiftcastOption { get; set; }

        public int NextPet { get; set; }
        /*
         * 0: Default
         * 1: Garuda
         * 2: Titan
         * 3: Ifrit
         */

        public int LucidDreamingPercentage { get; set; }

        public bool DelayOpeningBurst { get; set; }
        public void Reset()
        {
            EarlyDecisionMode = true;
            SwiftcastOption = 1;
            NextPet = 0;
            LucidDreamingPercentage = 50;
        }

        public bool Crimson { get; set; } = true;

        public bool SaveInstantSpells { get; set; } = false;
        public void ResetToggles()
        {
            Crimson = true;
            SaveInstantSpells = false;
        }

        public void OnLoad()
        {

        }
    }
}