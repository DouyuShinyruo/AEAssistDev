using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_HissatsuKyuten : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.HissatsuKyuten.IsReady()) return -1;
            if (ActionResourceManager.Samurai.Kenki < 25) return -1;
            var needUseAoe = TargetHelper.CheckNeedUseAOE(0, 5);
            if (!needUseAoe)
            {
                return -1;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuKyuten.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}