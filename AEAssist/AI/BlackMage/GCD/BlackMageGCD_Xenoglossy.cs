using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Xenoglossy : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // if we have polyglot
            if (ActionResourceManager.BlackMage.PolyglotCount > 0)
            {
                // if we are in ice
                if (ActionResourceManager.BlackMage.UmbralStacks > 0)
                {
                    // if we enter ice with transpose
                    if (ActionResourceManager.BlackMage.UmbralStacks < 3)
                    {
                        if (!BlackMageHelper.IsParadoxReady())
                        {
                            if (Core.Me.CurrentMana < 1600 * 3 + 800 + 400)
                            {
                                return 2;
                            }
                        }
                    }

                    // if we enter ice with blizzard3
                    if (ActionResourceManager.BlackMage.UmbralStacks == 3)
                    {
                        if (BlackMageHelper.WillPolyglotOverflow(TimeSpan.FromSeconds(8)))
                        {
                            return 3;
                        }
                    }

                    if (MovementManager.IsMoving)
                    {
                        return 10;
                    }
                }

                // if we are in fire
                if (ActionResourceManager.BlackMage.AstralStacks > 0)
                {
                    if (SpellsDefine.Amplifier.IsUnlock())
                    {
                        // if we have enough time for paradox
                        var baseGCDTime = SpellsDefine.Scathe.GetSpellEntity().SpellData.AdjustedCooldown
                            .Add(TimeSpan.FromMilliseconds(ConstValue.BlackMageLatencyCompensation));
                        if (ActionResourceManager.BlackMage.StackTimer > baseGCDTime +
                            BlackMageHelper.GetSpellCastTimeSpan(SpellsDefine.Despair.GetSpellEntity()))
                        {
                            // if we have more than 1 polyglot
                            // if we have less than 10 seconds to obtain another
                            // if we have Amplifier unlock, and it's gonna be ready in 10 seconds
                            if (BlackMageHelper.WillPolyglotOverflow(TimeSpan.FromSeconds(10)))
                            {
                                return 1;
                            }
                            if (MovementManager.IsMoving)
                            {
                                return 10;
                            }
                        }
                    }
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetXenoglossy();
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