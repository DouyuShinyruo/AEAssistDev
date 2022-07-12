using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.Ability
{
    public class SageAbilityTaurochole : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<SageSettings>().Heal)
            {
                return -5;
            }
            if (!SpellsDefine.Taurochole.IsReady() || ActionResourceManager.Sage.Addersgall == 0) return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Taurochole.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}