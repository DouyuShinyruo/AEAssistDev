using AEAssist.AI.Ninja.SpellQueue;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.GCD
{
    public class NinjaGCD_Raiton : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (SpellsDefine.Ten.GetSpellEntity().SpellData.Charges < 1)
            {
                return -10;
            }

            if (Core.Me.HasAura(AurasDefine.RaijuReady))
            {
                if (AuraHelper.GetAuraStack(Core.Me, AurasDefine.RaijuReady) == 3)
                {
                    return -5;
                }
            }
            if (SpellsDefine.Ten.GetSpellEntity().SpellData.Cooldown < TimeSpan.FromSeconds(5))
            {
                return 1;
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