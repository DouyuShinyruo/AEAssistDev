using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.GCD
{
    public class BardGCD_BlastArrow : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!AEAssist.DataBinding.Instance.UseApex)
                return -1;
            if (!Core.Me.HasAura(AurasDefine.BlastArrowReady))
                return -2;

            var aura = Core.Me.GetAuraById(AurasDefine.BlastArrowReady);
            var tar = Core.Me.CurrentTarget as Character;


            if (aura.TimespanLeft.TotalMilliseconds <= 2500)
                return 3;

            if (BardSpellHelper.IsTargetNeedIronJaws(Core.Me.CurrentTarget as Character, 3000))
                return -3;

            if (AIRoot.Instance.CloseBurst)
                return 1;

            if (TargetHelper.CheckNeedUseAOE(25, 2, ConstValue.BardAOECount))
                return 5;

            if (BardSpellHelper.Prepare2BurstBuffs()) return -4;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = BardSpellHelper.GetBlastArrow();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}