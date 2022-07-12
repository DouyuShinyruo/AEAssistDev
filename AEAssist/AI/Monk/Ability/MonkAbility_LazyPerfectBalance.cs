using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_LazyPerfectBalance : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!AEAssist.DataBinding.Instance.LazyOn)
            {
                return -5;
            }

            if (!AIRoot.Instance.CloseBurst)
                return -5;

            if (!SpellsDefine.PerfectBalance.IsUnlock())
            {
                return -10;
            }

            if (!SpellsDefine.PerfectBalance.IsReady())
            {
                return -1;
            }

            if (SpellsDefine.PerfectBalance.RecentlyUsed() || Core.Me.HasAura(AurasDefine.PerfectBalance))
            {
                return -2;
            }

            if (ActionResourceManager.Monk.BlitzTimer != TimeSpan.Zero)
            {
                return -5;
            }

            if (Core.Me.HasAura(AurasDefine.FormlessFist) || MonkSpellHelper.LastSpellWasNadi())
            {
                return -4;
            }

            if (!MonkSpellHelper.InRaptorForm())
            {
                return -3;
            }



            var target = Core.Me.CurrentTarget as Character;

            if (ActionResourceManager.Monk.ActiveNadi != ActionResourceManager.Monk.Nadi.Both)
            {
                if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Lunar)
                {
                    AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Solar;
                    return 1;
                }
                else if (ActionResourceManager.Monk.ActiveNadi == ActionResourceManager.Monk.Nadi.Solar &&
                         target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 15000) &&
                         Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 10000))
                {
                    AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Lunar;
                    return 1;
                }
                else
                {
                    if (target.HasMyAuraWithTimeleft(AurasDefine.Demolish, 15000) &&
                        Core.Me.HasMyAuraWithTimeleft(AurasDefine.DisciplinedFist, 10000))
                    {
                        AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Lunar;
                        return 3;
                    }

                    AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.Solar;
                    return 1;
                }

            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.PerfectBalance.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}