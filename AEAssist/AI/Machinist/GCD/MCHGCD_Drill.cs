using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.GCD
{
    public class MCHGCD_Drill : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Drill.IsReady())
                return -1;

            // 整备只有1层的时候,如果3秒内能冷却好,等一下
            if (!SpellsDefine.Reassemble.RecentlyUsed()
                && SpellsDefine.Reassemble.GetSpellEntity().SpellData.MaxCharges < 1.5f
                && SpellsDefine.Reassemble.GetSpellEntity().Cooldown.TotalMilliseconds < 3000)
                return -3;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = MCHSpellHelper.GetDrillIfWithAOE().GetSpellEntity();

            if (await spell.DoGCD()) return spell;

            return null;
        }
    }
}