using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.GCD
{
    public class BardGCD_BaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastGCD)
        {
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (DataBinding.Instance.UseAOE)
            {
                var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me.CurrentTarget, 12, 6);
                if (aoeCount == ConstValue.BardAOECount)
                {
                    var spellData = BardSpellHelper.GetBaseGCD();
                    if (spellData.Id == SpellsDefine.RefulgentArrow)
                    {
                        if (await spellData.DoGCD()) return spellData;
                    }
                    else
                    {
                        spellData = BardSpellHelper.GetQuickNock();
                        if (spellData == null)
                            return null;
                        if (await spellData.DoGCD())
                            return spellData;
                    }
                }
                else if (aoeCount > ConstValue.BardAOECount)
                {
                    var spellData = BardSpellHelper.GetQuickNock();
                    if (spellData == null)
                        return null;
                    if (await spellData.DoGCD())
                        return spellData;
                }
            }

            {
                var spellData = BardSpellHelper.GetBaseGCD();
                if (spellData == null)
                    return null;
                if (await spellData.DoGCD())
                    return spellData;
            }

            return null;
        }
    }
}