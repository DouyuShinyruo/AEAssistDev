using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Fire2 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // only cast if setting allow aoe
            if (!DataBinding.Instance.UseAOE)
            {
                return -1;
            }

            if (!SpellsDefine.Fire2.IsUnlock())
            {
                return -2;
            }


            // check if there is enough targets for us to do aoe
            var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.CurrentTarget, 25, 5);
            if (aoeCount >= 3)
            {
                // if we can cast Flare, we save mana for doing so
                if (SpellsDefine.Flare.IsUnlock())
                {
                    // todo： find a way to do calculation by rb, or anything but human
                    // if we are in fire 
                    if (ActionResourceManager.BlackMage.AstralStacks > 0)
                    {
                        if (ActionResourceManager.BlackMage.UmbralHearts > 0 && Core.Me.CurrentMana >= 1500 + 800)
                        {
                            return 1;
                        }

                        if (Core.Me.CurrentMana >= 3000 + 800)
                        {
                            return 2;
                        }
                    }
                    // if we are in ice, we use this to replace what ever fire3 is
                    // if we are in ice
                    if (ActionResourceManager.BlackMage.UmbralStacks > 0)
                    {
                        // if we used blizzard4, paradox, and max mana
                        if (BlackMageHelper.IsUmbralFinished())
                        {
                            return 2;
                        }
                    }

                    // if we are in nothing, we go to fire
                    if (ActionResourceManager.BlackMage.UmbralStacks == 0 &&
                        ActionResourceManager.BlackMage.AstralStacks == 0)
                    {
                        // if we are at the beginning of fight 
                        if (Core.Me.CurrentMana == 10000)
                        {
                            return 4;
                        }
                    }
                }
                // if we can't do Flare, just do it
                else
                {
                    return 0;
                }

            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetFire2();
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