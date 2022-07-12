using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.Ability
{
    public class BlackMageAblity_Transpose : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // lastspell => last ability spell

            var lastGCDSpell = ActionManager.LastSpellId;
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;

            if (!SpellsDefine.Transpose.IsReady())
            {
                return -2;
            }

            // prevent redundant casting
            if (bdls == SpellsDefine.Blizzard3.GetSpellEntity() ||
                bdls == SpellsDefine.HighBlizzardII.GetSpellEntity() ||
                // lastGCDSpell == SpellsDefine.Paradox ||
                bdls == SpellsDefine.Fire.GetSpellEntity()
               )
            {
                return -1;
            }

            // if we are in ice, we have 3 UmbralHearts, we have used paradox, and full mana, and we have firestarter buff
            if (Core.Me.HasAura(AurasDefine.FireStarter) &&
                Core.Me.CurrentMana >= 400 + 1600 * 3 + 800 &&
                ActionResourceManager.BlackMage.UmbralStacks > 0 &&
                !BlackMageHelper.IsParadoxReady() &&
                bdls != SpellsDefine.Fire3.GetSpellEntity())
            {
                return 1;
            }

            // if we are in fire, we have polyglot to go
            if (ActionResourceManager.BlackMage.AstralStacks > 0 &&
                Core.Me.CurrentMana == 0 &&
                BlackMageHelper.IsTargetNeedThunder(Core.Me.CurrentTarget as Character, 10000))
            {
                return 2;
            }
            // if we don't have target
            if (!Core.Me.HasTarget || !Core.Me.CurrentTarget.CanAttack)
            {
                // and we are in fire
                if (ActionResourceManager.BlackMage.AstralStacks > 0)
                {
                    return 3;
                }
                // if we are in ice, with 3 UmbralHearts, we go to fire and come back to get paradox
                if (BlackMageHelper.UmbralHeatsReady() &&
                    ActionResourceManager.BlackMage.UmbralStacks > 0)
                {
                    return 4;
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Transpose.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}