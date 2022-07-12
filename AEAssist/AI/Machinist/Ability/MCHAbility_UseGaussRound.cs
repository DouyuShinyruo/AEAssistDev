using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.Ability
{
    public class MCHAbility_UseGaussRound : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.GaussRound.IsReady() && !SpellsDefine.Ricochet.IsReady())
                return -1;

            if (SpellsDefine.GaussRound.IsMaxChargeReady() || SpellsDefine.Ricochet.IsMaxChargeReady()) return 1;

            if (SpellsDefine.BarrelStabilizer.IsUnlock())
            {
                if (SpellsDefine.BarrelStabilizer.IsReady())
                    return -2;
                var lastGCDIndex = SpellHistoryHelper.GetLastGCDIndex(SpellsDefine.BarrelStabilizer);
                if (AIRoot.GetBattleData<BattleData>().lastGCDIndex - lastGCDIndex < 2) return -3;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            SpellEntity spellData;
            if (SpellsDefine.GaussRound.GetSpellEntity().SpellData.Charges >=
                SpellsDefine.Ricochet.GetSpellEntity().SpellData.Charges)
                spellData = SpellsDefine.GaussRound.GetSpellEntity();
            else
                spellData = SpellsDefine.Ricochet.GetSpellEntity();

            if (await spellData.DoAbility()) return spellData;

            return null;
        }
    }
}