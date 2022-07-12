using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Fire4 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var minmana = 1600;
            if (SpellsDefine.Despair.IsUnlock())
            {
                minmana = 2400;
            }

            if (!SpellsDefine.Fire4.IsUnlock())
            {
                // if (ActionResourceManager.BlackMage.AstralStacks > 0 && ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds > 5000)
                if (ActionResourceManager.BlackMage.AstralStacks > 0 && ActionResourceManager.BlackMage.StackTimer > BlackMageHelper.GetSpellCastTimeSpan(SpellsDefine.Fire.GetSpellEntity()))
                {
                    return 3;
                }
            }

            if (Core.Me.CurrentMana >= minmana &&
                BlackMageHelper.IsMaxAstralStacks())
            {
                // if (ActionResourceManager.BlackMage.StackTimer.TotalMilliseconds > 5500)
                var baseGCDTime = SpellsDefine.Scathe.GetSpellEntity().SpellData.AdjustedCooldown.Add(TimeSpan.FromMilliseconds(ConstValue.BlackMageLatencyCompensation));
                var Fire4CastTime = BlackMageHelper.GetSpellCastTimeSpan(SpellsDefine.Fire4.GetSpellEntity());
                if (Fire4CastTime < TimeSpan.FromMilliseconds(2000))
                { Fire4CastTime = baseGCDTime; }
                var ParadoxCastTime = BlackMageHelper.GetSpellCastTimeSpan(SpellsDefine.Fire.GetSpellEntity());
                if (ParadoxCastTime < TimeSpan.FromMilliseconds(2000))
                { ParadoxCastTime = baseGCDTime; }

                if (ActionResourceManager.BlackMage.StackTimer > Fire4CastTime + ParadoxCastTime)
                {
                    return 2;
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetFire4();
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