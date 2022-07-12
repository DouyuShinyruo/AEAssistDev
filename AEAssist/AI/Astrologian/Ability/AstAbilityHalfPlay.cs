using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityHalfPlay : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            if (!SpellsDefine.Play.IsReady()) return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (!(Core.Me.HasAura(AurasDefine.ArrowDrawn) || Core.Me.HasAura(AurasDefine.BalanceDrawn) || Core.Me.HasAura(AurasDefine.SpearDrawn) || Core.Me.HasAura(AurasDefine.BoleDrawn) && Core.Me.HasAura(AurasDefine.EwerDrawn) || Core.Me.HasAura(AurasDefine.SpireDrawn))) return -2;

            if (SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds > 55 && SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds < 65)
            {
                if (SettingMgr.GetSetting<AstSettings>().AstHalfCard)
                {
                    SettingMgr.GetSetting<AstSettings>().AstHalfCard = false;
                    return 0;
                }

            }
            return -3;

        }

        public Task<SpellEntity> Run()
        {
            if (Core.Me.HasAura(AurasDefine.BalanceDrawn))
            {
                LogHelper.Debug("发近战太阳神");
                return AstSpellHelper.CastMeleeCardHalf();
            }
            if (Core.Me.HasAura(AurasDefine.BoleDrawn))
            {
                LogHelper.Debug("发远程世界树");
                return AstSpellHelper.CastRangedCardHalf();
            }
            if (Core.Me.HasAura(AurasDefine.ArrowDrawn))
            {
                LogHelper.Debug("发近战放浪神");
                return AstSpellHelper.CastMeleeCardHalf();
            }
            if (Core.Me.HasAura(AurasDefine.SpearDrawn))
            {
                LogHelper.Debug("发近战战争神");
                return AstSpellHelper.CastMeleeCardHalf();
            }
            if (Core.Me.HasAura(AurasDefine.EwerDrawn))
            {
                LogHelper.Debug("发远程河流神");
                return AstSpellHelper.CastRangedCardHalf();
            }
            if (Core.Me.HasAura(AurasDefine.SpireDrawn))
            {
                LogHelper.Debug("发远程建筑神");
                return AstSpellHelper.CastRangedCardHalf();
            }
            return null;
        }
    }
}
