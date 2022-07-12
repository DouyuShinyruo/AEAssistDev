using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.GCD
{
    internal class SageGCDEukrasianDiagnosis : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<SageSettings>().Heal)
            {
                return -2;
            }
            if (!MovementManager.IsMoving) return -1;
            return 0;
        }

        public Task<SpellEntity> Run()
        {
            LogHelper.Debug("刷单盾");
            //Character character = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && !r.HasAura(AurasDefine.EukrasianDiagnosis) && !r.HasAura(AurasDefine.DifferentialDiagnosis));
            //var spell = SageSpellHelper.CastEukrasianDiagnosis(character);
            return SageSpellHelper.CastEukrasianDiagnosisTest();

        }
    }
}
