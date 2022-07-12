using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.Ability
{
    public class NinjaAbility_Assassinate : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Assassinate.IsUnlock())
            {
                return -10;
            }

            if (SpellsDefine.DreamWithinaDream.IsUnlock())
            {
                if (!SpellsDefine.DreamWithinaDream.IsReady())
                {
                    return -1;
                }
            }
            else if (!SpellsDefine.Assassinate.IsReady())
            {
                return -1;
            }
            var target = Core.Me.CurrentTarget as Character;
            if (target.ContainAura(AurasDefine.VulnerabilityTrickAttack))
            {
                return 0;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Assassinate.GetSpellEntity();
            if (SpellsDefine.DreamWithinaDream.IsUnlock())
            {
                spell = SpellsDefine.DreamWithinaDream.GetSpellEntity();
            }
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}