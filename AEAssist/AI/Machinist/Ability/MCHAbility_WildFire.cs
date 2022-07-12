using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.Ability
{
    public class MCHAbility_WildFire : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Wildfire.IsReady())
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (!DataBinding.Instance.Wildfire)
                return -100;
            if (ActionResourceManager.Machinist.Heat < 50)
                return -3;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -10;
            if (ActionResourceManager.Machinist.OverheatRemaining.TotalMilliseconds > 0
                || SpellsDefine.Hypercharge.RecentlyUsed())
                return -4;
            if (SpellsDefine.BarrelStabilizer.IsReady())
                return -101;
            if (!SettingMgr.GetSetting<MCHSettings>().WildfireFirst &&
                MCHSpellHelper.CheckReassmableGCD(SettingMgr.GetSetting<MCHSettings>().StrongGCDCheckTime))
                return -5;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.Wildfire.DoAbility()) return SpellsDefine.Wildfire.GetSpellEntity();

            return null;
        }
    }
}