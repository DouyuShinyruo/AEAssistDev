using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class GunBreakerSettings : IBaseSetting
    {
        public GunBreakerSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }
        public int UsePotionEarly { get; set; }
        public float NotUseRoughDivide { get; set; } = 2.5f;

        public void Reset()
        {
            EarlyDecisionMode = true;

        }

        public void OnLoad()
        {

        }
    }
}