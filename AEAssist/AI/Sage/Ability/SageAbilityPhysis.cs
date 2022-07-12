using AEAssist.Define;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.Ability
{
    public class SageAbilityPhysisII : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<SageSettings>().Heal)
            {
                return -5;
            }
            var physisCheck = SageSpellHelper.GetPhysis();
            if (physisCheck == null) return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SageSpellHelper.GetPhysis();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}