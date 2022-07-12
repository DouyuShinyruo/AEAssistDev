using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.GCD
{
    internal class AstGCDDot : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var tar = Core.Me.CurrentTarget as Character;
            if (!AEAssist.DataBinding.Instance.UseDot)
                return -1;
            if (TTKHelper.IsTargetTTK(tar))
                return -2;

            if (DotBlacklistHelper.IsBlackList(Core.Me.CurrentTarget as Character))
                return -10;

            var dots = 0;
            if (AstSpellHelper.IsTargetHasAuraCombust(tar)) dots++;
            if (dots < 1) return 0;
            var timeLeft = SettingMgr.GetSetting<AstSettings>().Dot_TimeLeft;
            if (AstSpellHelper.IsTargetNeedCombust(tar, timeLeft)) return 1;
            return -3;

        }

        public async Task<SpellEntity> Run()
        {

            var spell = AstSpellHelper.GetCombust();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}
