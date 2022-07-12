using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.GCD
{
    public class MCHGCD_AirAnchor : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var spell = MCHSpellHelper.GetAirAnchor();
            if (spell == 0 || !spell.IsReady())
                return -1;


            // 只有热弹的时候,给钻头让路
            if (spell == SpellsDefine.HotShot && SpellsDefine.Drill.IsReady())
                return -2;

            // 整备只有1层的时候,如果5秒内能冷却好,等一下
            if (!SpellsDefine.Reassemble.RecentlyUsed()
                && SpellsDefine.Reassemble.GetSpellEntity().SpellData.MaxCharges < 1.5f
                && SpellsDefine.Reassemble.GetSpellEntity().Cooldown.TotalMilliseconds < 5000)
                return -3;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = MCHSpellHelper.GetAirAnchor().GetSpellEntity();

            if (await spell.DoGCD()) return spell;

            return null;
        }
    }
}