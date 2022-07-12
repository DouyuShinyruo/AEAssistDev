using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.GCD
{
    public class MCHGCD_UnderHyperCharge : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (SpellsDefine.Hypercharge.RecentlyUsed() ||
               (ActionResourceManager.Machinist.OverheatRemaining.TotalMilliseconds > 0 && AIRoot.GetBattleData<MCHBattleData>().HyperchargeGCDCount < 5))
            {
                return 0;
            }

            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = MCHSpellHelper.GetUnderHyperChargeGCD();
            if (await spell.DoGCD()) return spell;

            return null;
        }
    }
}