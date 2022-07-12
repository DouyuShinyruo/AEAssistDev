using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.Ability
{
    public class ReaperAbility_TrueNorth : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!AEAssist.DataBinding.Instance.UseTrueNorth)
                return -10;

            if (!SpellsDefine.TrueNorth.IsReady())
                return -1;

            if (Core.Me.HasAura(AurasDefine.TrueNorth)
                || SpellsDefine.TrueNorth.RecentlyUsed())
                return -9;

            if (!SpellsDefine.Gibbet.IsUnlock()) return -2;

            if (!SpellsDefine.BloodStalk.RecentlyUsed()
                && !Core.Me.HasAura(AurasDefine.SoulReaver)
                && !SpellsDefine.Gluttony.RecentlyUsed()
                && !SpellsDefine.GrimSwathe.RecentlyUsed())
                return -3;

            //alway use in the second half of GCD
            if (!AIRoot.Instance.Is2ndAbilityTime())
                return -11;

            var spell = ReaperSpellHelper.Gibbit_Gallows(Core.Me.CurrentTarget);
            if (spell == null)
                return -4;

            if (spell.Id == SpellsDefine.Gibbet && !Core.Me.CurrentTarget.IsFlanking)
                return 1;

            if (spell.Id == SpellsDefine.Gallows && !Core.Me.CurrentTarget.IsBehind)
                return 2;

            return -5;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.TrueNorth.GetSpellEntity();
            if (await spell.DoAbility()) return spell;

            return null;
        }
    }
}