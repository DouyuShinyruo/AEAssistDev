using AEAssist.Define;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Dot : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return BlackMageHelper.ThunderCheck();
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetThunder();
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