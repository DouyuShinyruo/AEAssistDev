using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.Ability
{
    public class SMNAbility_UsePotion : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -4;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<GeneralSettings>().IntPotionId))
                return -6;
            
            if (ActionResourceManager.Summoner.TranceTimer >= (int)AIRoot.Instance.GetGCDDuration() *2 && !SMN_SpellHelper.AnyPet() && !SMN_SpellHelper.PhoenixTrance())
                return 0;

            return -7;
        }

        public async Task<SpellEntity> Run()
        {
            var ret = await PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().IntPotionId);
            if (ret) AIRoot.Instance.MuteAbilityTime();

            await Task.CompletedTask;
            return null;
        }
    }
}