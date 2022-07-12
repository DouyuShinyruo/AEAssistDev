using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.Ability
{
    public class MCHAbility_UsePotion : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (!DataBinding.Instance.Wildfire)
                return -100;
            if (AIRoot.GetBattleData<BattleData>().maxAbilityTimes <
                SettingMgr.GetSetting<GeneralSettings>().MaxAbilityTimsInGCD)
                return -3;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -4;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId))
                return -5;

            if (SpellsDefine.Wildfire.RecentlyUsed() ||
                SpellsDefine.Wildfire.GetSpellEntity().Cooldown.TotalMilliseconds < 10000)
                return 0;
            return -10;
        }

        public async Task<SpellEntity> Run()
        {
            var ret = await PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId);
            if (ret) AIRoot.Instance.MuteAbilityTime();

            await Task.CompletedTask;
            return null;
        }
    }
}