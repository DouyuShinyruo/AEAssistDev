using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.GCD
{
    public class MonkGCD_PerfectBalanceGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Monk.BlitzTimer != TimeSpan.Zero)
            {
                AIRoot.GetBattleData<MonkBattleData>().CurrentNadiCombo = MonkNadiCombo.None;
                return -2;
            }
            if (SpellsDefine.PerfectBalance.RecentlyUsed() || Core.Me.HasAura(AurasDefine.PerfectBalance))
            {
                return 1;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var target = Core.Me.CurrentTarget as Character;
            return await MonkSpellHelper.PerfectBalanceGCDCombo(target);
        }
    }
}