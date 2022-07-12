using AEAssist.AI.Ninja.SpellQueue;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.GCD
{
    public class NinjaGCD_Suiton : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (SpellsDefine.TrickAttack.GetSpellEntity().Cooldown < TimeSpan.FromSeconds(19))
            {
                if (!Core.Me.HasAura(AurasDefine.Suiton))
                {
                    return 1;
                }
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            AISpellQueueMgr.Instance.Apply<SpellQueue_Suiton>();
            await Task.CompletedTask;
            return null;
        }
    }
}