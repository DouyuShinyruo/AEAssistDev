using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.Ability
{
    public class NinjaAbility_TrickAttack : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.TrickAttack.IsUnlock())
            {
                return -10;
            }
            var target = Core.Me.CurrentTarget as Character;
            if (SpellsDefine.TrickAttack.IsReady() &&
                (Core.Me.HasAura(AurasDefine.Suiton) || AIRoot.GetBattleData<BattleData>().lastGCDSpell == SpellsDefine.Suiton.GetSpellEntity()))
            {
                return 0;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.TrickAttack.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}