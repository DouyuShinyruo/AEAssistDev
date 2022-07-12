using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.GCD
{
    public class BardGCD_Dot : IAIHandler
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
            if (BardSpellHelper.IsTargetHasAura_WindBite(tar)) dots++;

            if (BardSpellHelper.IsTargetHasAura_VenomousBite(tar)) dots++;

            if (dots >= 2)
            {
                var timeLeft = SettingMgr.GetSetting<BardSettings>().Dot_TimeLeft;
                if (BardSpellHelper.IsTargetNeedIronJaws(tar, timeLeft))
                    return 1;
                return -3;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            SpellEntity spell = null;
            var timeLeft = SettingMgr.GetSetting<BardSettings>().Dot_TimeLeft;
            var target = Core.Me.CurrentTarget as Character;
            if (!BardSpellHelper.IsTargetHasAura_WindBite(target))
                spell = BardSpellHelper.GetWindBite();
            else if (!BardSpellHelper.IsTargetHasAura_VenomousBite(target))
                spell = BardSpellHelper.GetVenomousBite();
            else if (BardSpellHelper.IsTargetNeedIronJaws(target, timeLeft))
                spell = SpellsDefine.IronJaws.GetSpellEntity();

            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret) return spell;

            return null;
        }
    }
}