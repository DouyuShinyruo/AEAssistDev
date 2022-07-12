using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.Ability
{
    public class MCHAbility_FinalBurst : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var queen = MCHSpellHelper.GetAutomatonQueen();
            if (!queen.IsUnlock())
                return -1;
            var wildfire = SpellsDefine.Wildfire;
            if (!wildfire.IsUnlock())
                return -2;
            if (!AEAssist.DataBinding.Instance.FinalBurst)
                return -3;
            if (ActionResourceManager.Machinist.SummonRemaining.TotalMilliseconds == 0
                && !Core.Me.HasAura(AurasDefine.WildfireBuff))
                return -4;

            SpellEntity spell = null;
            if (ActionResourceManager.Machinist.SummonRemaining.TotalMilliseconds > 0)
                spell = MCHSpellHelper.GetQueenOverdrive();
            else if (Core.Me.HasAura(AurasDefine.WildfireBuff)) spell = SpellsDefine.Detonator.GetSpellEntity();
            if (!spell.IsReady())
                return -5;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            SpellEntity spell = null;
            if (ActionResourceManager.Machinist.SummonRemaining.TotalMilliseconds > 0)
                spell = MCHSpellHelper.GetQueenOverdrive();
            else if (Core.Me.HasAura(AurasDefine.WildfireBuff)) spell = SpellsDefine.Detonator.GetSpellEntity();

            if (spell == null)
                return null;
            if (await spell.DoAbility()) return spell;

            return null;
        }
    }
}