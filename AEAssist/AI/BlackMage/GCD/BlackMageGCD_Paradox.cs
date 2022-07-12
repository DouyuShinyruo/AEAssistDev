using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Paradox : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // prevent redundant casting
            var lastGCDSpell = BlackMageHelper.GetLastSpell();
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;


            if (bdls == SpellsDefine.Fire.GetSpellEntity() ||
                lastGCDSpell == SpellsDefine.Paradox
               )
            {
                return -10;
            }



            //low level - no paradox
            // if we are in fire, use fire to refresh AF timer
            // if we have less than 2400 mana, skip refresh buff, go despair
            if (!SpellsDefine.Paradox.IsUnlock())
            {
                if (ActionResourceManager.BlackMage.AstralStacks > 0 &&
                    Core.Me.CurrentMana > 2400 + 1600)
                {
                    return 1;
                }

                return -1;
            }

            // with paradox
            if (!BlackMageHelper.IsParadoxReady())
            {
                return -1;
            }

            if (ActionResourceManager.BlackMage.AstralStacks > 0 &&
                Core.Me.CurrentMana > 2400 + 1600)
            {
                return 1;
            }
            // if we are in ice
            if (ActionResourceManager.BlackMage.UmbralStacks > 0)
            {
                return 2;
            }



            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetParadox();
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