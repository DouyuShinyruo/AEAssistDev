using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.GCD
{
    public class ReaperGCD_BaseGCDCombo : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // DoubleEnshroudPrefer. cant use baseCombo, so use this
            if (SpellsDefine.Enshroud.RecentlyUsed() || Core.Me.HasMyAuraWithTimeleft(AurasDefine.Enshrouded))
            {
                var spell = ReaperSpellHelper.GetShadowOfDeath();
                if (await spell.DoGCD())
                    return spell;
            }

            return await ReaperSpellHelper.BaseGCDCombo(Core.Me.CurrentTarget);
        }
    }
}