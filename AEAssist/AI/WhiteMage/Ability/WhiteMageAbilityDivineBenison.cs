using AEAssist.Define;
using AEAssist.Helper;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage.Ability
{
    internal class WhiteMageAbilityDivineBenison : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<WhiteMageSettings>().Heal)
            {
                return -5;
            }
            if (!SpellsDefine.DivineBenison.IsReady()) return -1;
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().DivineBenisonHp);
            if (skillTarget == null)
            {
                return -2;
            }
            if (!SpellsDefine.DivineBenison.IsMaxChargeReady())
            {
                return -1;
            }
            return 0;
        }

        public Task<SpellEntity> Run()
        {
            return WhiteMageSpellHelper.CastDivineBenison();
        }
    }
}