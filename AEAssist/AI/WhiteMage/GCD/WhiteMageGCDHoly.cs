using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Globalization;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage.GCD
{
    public class WhiteMageGCDHoly : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var holy = WhiteMageSpellHelper.GetHoly();
            var aoeChecker = TargetHelper.CheckNeedUseAOE(8, 8, ConstValue.WhiteMageAOECount);
            if (MovementManager.IsMoving) return -1;
            if (!aoeChecker) return -2;
            if (holy == null) return -3;
            var distanceToEnemy = Core.Me.CurrentTarget.Distance();
            if (!(distanceToEnemy > 8)) return 0;
            LogHelper.Debug("Distance is greater than 8 so skipping Holy.");
            LogHelper.Debug(distanceToEnemy.ToString(CultureInfo.InvariantCulture));

            return -1;
        }

        public async Task<SpellEntity> Run()
        {

            var spell = WhiteMageSpellHelper.GetHoly();
            //spell.SpellTargetType = SpellTargetType.Self;
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}
