using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_UmbralSoul : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // if there is no target and we are in ice, try to get more Umbral Hearts
            if (!Core.Me.HasTarget || !Core.Me.CurrentTarget.CanAttack)
            {
                if (ActionResourceManager.BlackMage.UmbralStacks > 0)
                {
                    return 1;
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.UmbralSoul.GetSpellEntity();
            if (spell == null)
                return null;
            if (MovementManager.IsMoving && spell.SpellData.AdjustedCastTime > TimeSpan.Zero)
            {
                return null;
            }
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}