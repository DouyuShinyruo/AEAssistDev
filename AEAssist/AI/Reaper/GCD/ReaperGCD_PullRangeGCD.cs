using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.GCD
{
    public class ReaperGCD_PullRangeGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget, 6))
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Harpe;
            if (SpellsDefine.HarvestMoon.IsUnlock() && Core.Me.HasAura(AurasDefine.Soulsow))
                spell = SpellsDefine.HarvestMoon;
            else if (!AEAssist.SettingMgr.GetSetting<ReaperSettings>().UseHarpe) return null;

            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}