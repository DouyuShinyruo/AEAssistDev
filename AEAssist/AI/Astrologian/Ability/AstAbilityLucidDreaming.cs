using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityLucidDreaming : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.LucidDreaming.IsReady() || Core.Me.HasAura(AurasDefine.LucidDreaming)) return -1;
            if (Core.Me.CurrentManaPercent >= SettingMgr.GetSetting<AstSettings>().LucidDreamingTrigger) return -1;
            if (!SettingMgr.GetSetting<AstSettings>().LucidDreamingToggle) return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.LucidDreaming.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
