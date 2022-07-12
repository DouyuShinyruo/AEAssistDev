using AEAssist.AI.Ninja.SpellQueue;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.GCD
{
    public class NinjaGCD_Kassatsu : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var target = Core.Me.CurrentTarget as Character;
            if (target.ContainAura(AurasDefine.VulnerabilityTrickAttack))
            {
                if (Core.Me.HasAura(AurasDefine.Kassatsu))
                {
                    return 0;
                }
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            if (TargetHelper.CheckNeedUseAOE(Core.Me.CurrentTarget, 25, 6, 3))
            {
                AISpellQueueMgr.Instance.Apply<SpellQueue_GokaMekkyaku>();
                await Task.CompletedTask;
                return null;
            }
            AISpellQueueMgr.Instance.Apply<SpellQueue_HyoshoRanryu>();
            await Task.CompletedTask;
            return null;

        }
    }
}