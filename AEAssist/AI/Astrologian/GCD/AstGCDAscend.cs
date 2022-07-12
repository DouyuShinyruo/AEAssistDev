using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.GCD
{
    internal class AstGCDAscend : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            LogHelper.Debug("Checking if SwiftRes Toggle is on...");
            if (!SettingMgr.GetSetting<AstSettings>().SwiftResToggle) return -3;
            LogHelper.Debug("Checking if swiftcast is ready");
            if (!SpellsDefine.Swiftcast.IsReady()) return -5;
            LogHelper.Debug("checking if allies are dead");
            if (GroupHelper.DeadAllies.Count == 0) return -4;
            return 0;
        }

        public Task<SpellEntity> Run()
        {
            return AstSpellHelper.CastResPriority();
        }
    }
}
