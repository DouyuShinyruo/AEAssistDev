using AEAssist.Define;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.Ability
{
    public class GunBreakerAbility_Continuation : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (Core.Me.HasAura(AurasDefine.ReadytoBlast))
                return 1;
            if (Core.Me.HasAura(AurasDefine.ReadytoRip))
                return 2;
            if (Core.Me.HasAura(AurasDefine.ReadytoGouge))
                return 3;
            if (Core.Me.HasAura(AurasDefine.ReadytoTear))
                return 4;
            return -1;
        }
        public async Task<SpellEntity> Run()
        {
            var spell = GunBreakerSpellHelper.GetContinuation();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}