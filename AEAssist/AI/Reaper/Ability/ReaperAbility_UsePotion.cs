using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.Ability
{
    public class ReaperAbility_UsePotion : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -3;

            if (AIRoot.GetBattleData<BattleData>().maxAbilityTimes <
                SettingMgr.GetSetting<GeneralSettings>().MaxAbilityTimsInGCD)
                return -4;

            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId))
                return -5;

            if (SpellsDefine.PlentifulHarvest.IsUnlock()
                && (SpellsDefine.ArcaneCircle.GetSpellEntity().Cooldown.TotalMilliseconds <= 2000
                    || Core.Me.HasMyAuraWithTimeleft(AurasDefine.ArcaneCircle)))
                return 1;


            if (!SpellsDefine.PlentifulHarvest.IsUnlock() && ActionResourceManager.Reaper.ShroudGauge >= 50)
                return 2;

            return -6;
        }

        public async Task<SpellEntity> Run()
        {
            var ret = await PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId);
            if (ret) AIRoot.Instance.MuteAbilityTime();

            await Task.CompletedTask;
            return null;
        }
    }
}