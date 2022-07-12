using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.GCD
{
    public class SageGcdToxikon : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var toxikonCheck = SageSpellHelper.GetToxikon();
            if (toxikonCheck == null) return -5;

            var battleData = AIRoot.GetBattleData<BattleData>();
            if (battleData.lastGCDSpell == SageSpellHelper.GetToxikon())
            {
                LogHelper.Debug("Toxikon last used skipping.");
                return -10;
            }

            if (MovementManager.IsMoving)
            {
                LogHelper.Debug("Player is moving so going to try and use Toxikon.");
                return 0;
            }

            var toxikonCharges = DataManager.GetSpellData(SpellsDefine.Toxikon).Charges;
            var toxikonIICharges = DataManager.GetSpellData(SpellsDefine.ToxikonII).Charges;

            if (!(toxikonCharges <= 1) && !(toxikonIICharges <= 1)) return 0;
            LogHelper.Debug("Toxikon's Didn't pass the check, currently only got Toxikon: " +
                            toxikonCharges + "and ToxikonII :" + toxikonIICharges + "charges.");
            return -4;

        }

        public async Task<SpellEntity> Run()
        {
            var spell = SageSpellHelper.GetToxikon();
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}