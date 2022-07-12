using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.Ability
{
    public class MonkAbility_UsePotion : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -4;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<GeneralSettings>().StrPotionId))
                return -6;

            if (SpellsDefine.Brotherhood.CoolDownInGCDs(10) && SpellsDefine.RiddleofFire.CoolDownInGCDs(3))
                return 0;

            return -7;
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