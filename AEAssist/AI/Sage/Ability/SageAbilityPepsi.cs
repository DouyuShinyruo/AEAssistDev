using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.Ability
{
    public class SageAbilityPepsi : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<SageSettings>().Heal)
            {
                return -5;
            }
            // this might not work at all since it will check for EukrasiaPrognosis and EukrasianDiagnosis
            // TODO: Might need a fix.
            if (!SpellsDefine.Pepsis.IsReady() || !Core.Me.HasAura(AurasDefine.EukrasianPrognosis) ||
                !Core.Me.HasAura(AurasDefine.EukrasianDiagnosis)) return -1;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Pepsis.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}