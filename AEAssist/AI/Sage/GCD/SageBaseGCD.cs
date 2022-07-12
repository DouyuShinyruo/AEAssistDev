using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.GCD
{
    public class SageBaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // don't cast dosis when moving, instead going to cast other spells.
            if (!MovementManager.IsMoving) return 0;

            // Always use Phlegma if there is more than one, no need to spam dosis.
            var phlegmaCharges = DataManager.GetSpellData(SpellsDefine.Phlegma).Charges;
            var phlegmaChargesII = DataManager.GetSpellData(SpellsDefine.PhlegmaII).Charges;
            var phlegmaChargesIII = DataManager.GetSpellData(SpellsDefine.PhlegmaIII).Charges;
            if (phlegmaCharges > 1 || phlegmaChargesII > 1 || phlegmaChargesIII > 1)
            {
                LogHelper.Debug("Skipping Dosis, since phlegma is greater than 1, better keep using it so we don't cap on it.");
                return -1;
            }
            LogHelper.Debug("Not casting Dosis, since we are moving.");
            return -10;

            // // prevent redundant casting
            // var battleData = AIRoot.GetBattleData<BattleData>();
            // if (battleData.lastGCDSpell == SpellsDefine.Dosis.GetSpellEntity() ||
            //     battleData.lastGCDSpell == SpellsDefine.DosisII.GetSpellEntity()||
            //     battleData.lastGCDSpell == SpellsDefine.DosisIII.GetSpellEntity()) {
            //     LogHelper.Info(battleData.lastGCDSpell.SpellData.LocalizedName);
            //     return -1;
            // }
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SageSpellHelper.GetBaseGcd();
            if (spell == null) return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}