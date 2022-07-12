using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityUsePotion:IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -4;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId))
                return -6;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var ret = await PotionHelper.UsePotion(SettingMgr.GetSetting<GeneralSettings>().MindPotionId);
            if (ret) AIRoot.Instance.MuteAbilityTime();

            await Task.CompletedTask;
            return null;
        }
    }
}
