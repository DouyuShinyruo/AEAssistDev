using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot.Enums;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class ScholarSettings : IBaseSetting
    {
        public ScholarSettings()
        {
            Reset();
        }
        public void Reset()
        {
            EarlyDecisionMode = false;//提前决策模式
            Dot_TimeLeft = ConstValue.AuraTick;
            TTK_Aero = 30;
            WhiteMageOpener = "Default";
            TetragrammatonHp = 80f;
            DivineBenisonHp = 90f;
            RegenHp = 80f;
        }
        public void OnLoad()
        {
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.WhiteMage] = WhiteMageOpener;
            LogHelper.Info($"WhiteMage Opener: {WhiteMageOpener}");
        }
        public int Dot_TimeLeft { get; set; } = ConstValue.AuraTick;
        public int TTK_Aero { get; set; }

        public int LucidDreamingTrigger { get; set; } = ConstValue.LucidDreamingDefaultRefresh;
        public bool LucidDreamingToggle { get; set; } = true;
        public bool SwiftResToggle { get; set; } = true;
        public bool Heal { get; set; } = true;
        public bool EarlyDecisionMode { get; set; }
        public string WhiteMageOpener { get; set; } = "Default";
        public int WhiteMageResPriority { get; set; } = 0;
        public float TetragrammatonHp { get; set; } = 80f;

        public float DivineBenisonHp { get; set; } = 90f;
        public float RegenHp { get; set; } = 80f;

        public float AfflatusSolaceHp { get; set; } = 70f;
    }
}
