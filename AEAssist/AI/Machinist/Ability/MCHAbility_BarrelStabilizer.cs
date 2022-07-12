using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.Ability
{
    public class MCHAbility_BarrelStabilizer : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.BarrelStabilizer.IsReady())
                return -1;
            if (ActionResourceManager.Machinist.Heat > 50)
                return -2;
            if (AIRoot.Instance.CloseBurst)
                return -3;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -4;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.BarrelStabilizer.DoAbility()) return SpellsDefine.BarrelStabilizer.GetSpellEntity();

            return null;
        }
    }
}