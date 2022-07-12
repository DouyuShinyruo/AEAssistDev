using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.GCD
{
    public class BlackMageGCD_Blizzard4 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // prevent redundant casting
            var bdls = AIRoot.GetBattleData<BattleData>();
            var lastGCDSpell = BlackMageHelper.GetLastSpell();
            if (
                bdls.lastGCDSpell == SpellsDefine.Blizzard4.GetSpellEntity() ||
                bdls.lastGCDSpell == SpellsDefine.Freeze.GetSpellEntity()
            )
            {
                return -10;
            }
            // prevent not learned skill or redundant casting
            if (BlackMageHelper.UmbralHeatsReady())
            {
                return -2;
            }

            if (ActionResourceManager.BlackMage.UmbralStacks == 3)
            {
                return 0;
            }

            // if we are in ice, should before paradox to prevent lag
            // lag -> paradox can't go at the very begining
            if (ActionResourceManager.BlackMage.UmbralStacks > 0 &&
                ActionResourceManager.BlackMage.UmbralStacks < 3)
            {
                return 1;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BlackMageHelper.GetBlizzard4();
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