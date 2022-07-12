using AEAssist.AI.Ninja.SpellQueue;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.GCD
{
    public class NinjaGCD_Raiju : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!Core.Me.HasAura(AurasDefine.RaijuReady))
            {
                return -10;
            }
            var target = Core.Me.CurrentTarget as Character;

            if (target.ContainAura(AurasDefine.VulnerabilityTrickAttack))
            {
                return 0;
            }
            return -4;

        }

        public async Task<SpellEntity> Run()
        {
            AISpellQueueMgr.Instance.Apply<SpellQueue_Raiton>();
            await Task.CompletedTask;
            return null;
        }
    }
}