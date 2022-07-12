using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.GCD
{
    public class ReaperGCD_ShadowOfDeath : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.ShadowOfDeath.IsUnlock())
                return -1;

            var target = Core.Me.CurrentTarget as Character;

            // 妖异
            if (Core.Me.HasAura(AurasDefine.SoulReaver))
                return -2;

            if (ReaperSpellHelper.PrepareEnterDoubleEnshroud())
                return -3;

            // if (!target.HasMyAuraWithTimeleft(AurasDefine.DeathsDesign, 10000)
            //     && (ActionResourceManager.Reaper.SoulGauge > 50
            //         || ActionResourceManager.Reaper.ShroudGauge > 50 || SpellsDefine.PlentifulHarvest.RecentlyUsed()))
            //     return 1;

            if (target.HasMyAuraWithTimeleft(AurasDefine.DeathsDesign, 3000))
                return -4;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = ReaperSpellHelper.GetShadowOfDeath();
            if (await spell.DoGCD())
                return spell;
            return null;
        }
    }
}