using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.Ability
{
    public class NinjaAbility_Bhavacakra : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Bhavacakra.IsUnlock())
            {
                return -10;
            }

            var target = Core.Me.CurrentTarget as Character;
            var currentCombo = AIRoot.GetBattleData<NinjaBattleData>().CurrCombo;
            var currentNinki = ActionResourceManager.Ninja.NinkiGauge;
            if (currentNinki >= 50)
            {
                if (target.ContainAura(AurasDefine.VulnerabilityTrickAttack))
                {
                    return 0;
                }

                if (SpellsDefine.Mug.CoolDownInGCDs(1))
                {
                    return 1;
                }

                if (currentCombo == NinjaComboStages.AeolianEdge)
                {
                    if (Core.Me.HasAura(AurasDefine.Bunshin))
                    {
                        if (20 + currentNinki > 100)
                        {
                            return 1;
                        }
                    }

                    else if (15 + currentNinki > 100)
                    {
                        return 1;
                    }
                }
                else
                {
                    if (Core.Me.HasAura(AurasDefine.Bunshin))
                    {
                        if (10 + currentNinki > 100)
                        {
                            return 1;
                        }
                    }
                    else if (5 + currentNinki > 100)
                    {
                        return 2;
                    }
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = NinjaSpellHelper.GetBhavacakra(Core.Me.CurrentTarget);
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}