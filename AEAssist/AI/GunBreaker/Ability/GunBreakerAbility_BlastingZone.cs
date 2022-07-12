using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
using ff14bot;

namespace AEAssist.AI.GunBreaker.Ability
{
    public class GunBreakerAbility_BlastingZone : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.BlastingZone.IsReady() || !SpellsDefine.DangerZone.IsReady())
                return -1;
            if (SpellsDefine.NoMercy.GetSpellEntity().SpellData.Charges < 0.9)
                return 1;
            if (!SpellsDefine.NoMercy.CoolDownInGCDs(2))
                return -2;
            if (SpellsDefine.NoMercy.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds < 6000)
                return -21;
            //if (SpellsDefine.NoMercy.CoolDownInGCDs(2))
            //    return -2;
            return 0;
        }
        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.BlastingZone.GetSpellEntity();
            if (spell == null) return null;
            if (Core.Me.ClassLevel < 90)
                if (Core.Me.ClassLevel > 17)
                    spell= SpellsDefine.DangerZone.GetSpellEntity();

            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}
