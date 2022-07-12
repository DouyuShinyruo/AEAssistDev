using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Blizzard3 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // prevent redundant casting
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            var lastGCDSpell = BlackMageHelper.GetLastSpell();
            if (bdls == SpellsDefine.Blizzard3.GetSpellEntity() ||
                bdls == SpellsDefine.HighBlizzardII.GetSpellEntity() ||
                lastGCDSpell == SpellsDefine.Paradox ||
                bdls == SpellsDefine.Fire.GetSpellEntity()
               )
            {
                return -10;
            }
            // prevent to waste mana font
            if (SpellsDefine.ManaFont.RecentlyUsed())
            {
                return -1;
            }
            // fire to ice
            if (BlackMageHelper.IsMaxAstralStacks())
            {
                return 1;
            }
            // if we are in nothing, starting to fight without full mana
            if (ActionResourceManager.BlackMage.AstralStacks == 0 &&
                ActionResourceManager.BlackMage.UmbralStacks == 0 &&
                Core.Me.CurrentMana < 10000)
            {
                return 2;
            }
            // this shouldn't run, but put here just in case I fucked up '
            if (ActionResourceManager.BlackMage.UmbralStacks > 0 &&
                ActionResourceManager.BlackMage.UmbralStacks < 3)
            {
                return 69;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetBlizzard3();
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