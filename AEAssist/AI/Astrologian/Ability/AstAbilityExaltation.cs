using AEAssist.Define;
using AEAssist.Helper;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityExaltation : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<AstSettings>().Heal)
            {
                return -5;
            }
            if (!SpellsDefine.Exaltation.IsReady()) return -1;
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 80f && r.IsTank());
            if (skillTarget == null)
            {
                return -2;
            }

            return 0;
        }

        public Task<SpellEntity> Run()
        {
            return AstSpellHelper.CastExaltation();
        }
    }
}
