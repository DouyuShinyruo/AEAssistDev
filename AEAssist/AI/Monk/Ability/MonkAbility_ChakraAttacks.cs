using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_ChakraAttacks : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Monk.ChakraCount < 5)
            {
                return -10;
            }

            if (!SpellsDefine.SteelPeak.IsUnlock())
            {
                return -2;
            }


            return 0;
        }

        private static SpellEntity GetChakraSpell()
        {
            var singeTarget = SpellsDefine.SteelPeak.GetSpellEntity();
            if (SpellsDefine.TheForbiddenChakra.IsUnlock())
            {
                singeTarget = SpellsDefine.TheForbiddenChakra.GetSpellEntity();
            }
            var multipleTargets = SpellsDefine.HowlingFist.GetSpellEntity();
            if (!SpellsDefine.HowlingFist.IsUnlock())
            {
                multipleTargets = null;
            }
            if (SpellsDefine.Enlightenment.IsUnlock())
            {
                multipleTargets = SpellsDefine.Enlightenment.GetSpellEntity();
            }
            var targetRequired = 2;
            if (singeTarget == SpellsDefine.TheForbiddenChakra.GetSpellEntity() &&
                multipleTargets == SpellsDefine.HowlingFist.GetSpellEntity())
            {
                targetRequired = 4;
            }

            if (multipleTargets == null)
            {
                return singeTarget;
            }

            if (TargetHelper.CheckNeedUseAOETest(Core.Me.CurrentTarget, 10, 5, targetRequired))
            {
                return multipleTargets;
            }

            return singeTarget;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = GetChakraSpell();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}