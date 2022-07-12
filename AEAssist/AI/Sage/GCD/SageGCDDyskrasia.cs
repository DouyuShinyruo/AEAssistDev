using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Globalization;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.GCD
{
    public class SageGCDDyskrasia : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var dyskrasia = SageSpellHelper.GetDyskrasia();
            if (dyskrasia == null) return -1;
            var distanceToEnemy = Core.Me.CurrentTarget.Distance();
            if (!(distanceToEnemy > 7)) return 0;
            LogHelper.Debug("Distance is greater than 7 so skipping Dykrasia.");
            LogHelper.Debug(distanceToEnemy.ToString(CultureInfo.InvariantCulture));
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(0, 5, ConstValue.SageAOECount);
            LogHelper.Debug("Checking Enemy if it's around: " + aoeChecker);
            if (!aoeChecker) return null;

            var spell = SageSpellHelper.GetDyskrasia();
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}