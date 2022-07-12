using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
using ff14bot;
using System;

namespace AEAssist.AI.GunBreaker.GCD
{
    public class UpdatePos : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (Core.Me.HasTarget)
            {
                MeleePosition.Intance.TargetDistance = (int)Math.Round(TargetHelper.GetTargetDistanceFromMeTest(Core.Me, Core.Me.CurrentTarget) * 100);
            }
            return -1;
        }

        public Task<SpellEntity> Run()
        {
            return null;
        }
    }
}
