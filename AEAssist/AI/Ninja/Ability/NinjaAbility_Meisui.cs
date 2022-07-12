using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.Ability
{
    public class NinjaAbility_Meisui : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Meisui.IsUnlock())
            {
                return -10;
            }

            if (Core.Me.HasAura(AurasDefine.Suiton) &&
                SpellsDefine.TrickAttack.GetSpellEntity().Cooldown > TimeSpan.FromSeconds(10))
            {
                return 1;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Meisui.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}