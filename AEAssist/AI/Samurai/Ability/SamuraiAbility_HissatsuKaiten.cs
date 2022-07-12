using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_HissatsuKaiten : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                return 0;
            }
            var bd = AIRoot.GetBattleData<SamuraiBattleData>();

            if (bd.CurrPhase == SamuraiPhase.OddMinutesBurstPhase)
            {
                if (SamuraiSpellHelper.SenCounts() == 1)
                {
                    var target = Core.Me.CurrentTarget as Character;
                    if (!target.HasMyAuraWithTimeleft(10000))
                    {
                        return 1;
                    }
                }
                if (bd.CurrPhase == SamuraiPhase.OddMinutesBurstPhase)
                {
                    if (!SamuraiSpellHelper.TargetNeedsDot(Core.Me.CurrentTarget as Character))
                    {
                        if (SamuraiSpellHelper.SenCounts() == 1)
                        {
                            return 0;
                        }
                    }
                }
            }
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuKaiten.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}