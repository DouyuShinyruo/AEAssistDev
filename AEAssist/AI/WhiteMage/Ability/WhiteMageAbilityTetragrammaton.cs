using AEAssist.Define;
using AEAssist.Helper;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage.Ability
{
    internal class WhiteMageAbilityTetragrammaton : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<WhiteMageSettings>().Heal)
            {
                return -5;
            }
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().TetragrammatonHp);
            if (skillTarget == null)
            {
                return -2;
            }
            if (!SpellsDefine.Tetragrammaton.IsReady()) return -1;

            return 0;
        }

        public Task<SpellEntity> Run()
        {
            return WhiteMageSpellHelper.CastTetragrammaton();
        }
    }
}
