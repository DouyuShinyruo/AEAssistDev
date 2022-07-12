using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.Ability
{
    public class GunBreakerAbility_Bloodfest : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!DataBinding.Instance.Burst)
                return -100;

            if (!SpellsDefine.Bloodfest.GetSpellEntity().SpellData.IsReady())
                return -1;

            if (ActionResourceManager.Gunbreaker.Cartridge != 0)
                return -2;

            return 0;
        }
        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Bloodfest.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}
