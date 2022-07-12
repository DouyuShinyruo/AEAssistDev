using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_UsePotion : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (TTKHelper.IsTargetTTK(Core.Me.CurrentTarget as Character))
                return -4;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<GeneralSettings>().DexPotionId))
                return -6;

            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.RagingStrikes)
                || SpellsDefine.RagingStrikes.GetSpellEntity().Cooldown.TotalMilliseconds < 5000)
                return 0;

            return -7;
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