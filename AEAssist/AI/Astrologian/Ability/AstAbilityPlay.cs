using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;


namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityPlay : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            if (!SpellsDefine.Play.IsReady()) return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (!(Core.Me.HasAura(AurasDefine.ArrowDrawn) || Core.Me.HasAura(AurasDefine.BalanceDrawn) || Core.Me.HasAura(AurasDefine.SpearDrawn) || Core.Me.HasAura(AurasDefine.BoleDrawn) || Core.Me.HasAura(AurasDefine.EwerDrawn) || Core.Me.HasAura(AurasDefine.SpireDrawn))) return -2;
            if (SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds < 110 && SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds >= 5)
            {

                if (SpellsDefine.Draw.IsMaxChargeReady(0.1f))
                {
                    LogHelper.Debug("即将溢出，发卡");
                    return 0;
                }
                return -3;
            }
            //if (Core.Me.HasAura(AurasDefine.TheArrow) && Core.Me.HasAura(AurasDefine.TheBalance) && Core.Me.HasAura(AurasDefine.TheSpear) && Core.Me.HasAura(AurasDefine.TheBole) && Core.Me.HasAura(AurasDefine.TheEwer) && Core.Me.HasAura(AurasDefine.TheSpire))
            //{
            //LogHelper.Debug("有卡不抽");
            //return -2;
            //}
            //if (SpellsDefine.Divination.CoolDownInGCDs(1))
            //{
            //SpellData
            //SpellsDefine.Divination.GetSpellEntity().AdjustedCooldown
            //}
            return 0;
        }

        public Task<SpellEntity> Run()
        {
            if (Core.Me.HasAura(AurasDefine.BalanceDrawn))
            {
                LogHelper.Debug("发近战太阳神");
                return AstSpellHelper.CastMeleeCard();
            }
            if (Core.Me.HasAura(AurasDefine.BoleDrawn))
            {
                LogHelper.Debug("发远程世界树");
                return AstSpellHelper.CastRangedCard();
            }
            if (Core.Me.HasAura(AurasDefine.ArrowDrawn))
            {
                LogHelper.Debug("发近战放浪神");
                return AstSpellHelper.CastMeleeCard();
            }
            if (Core.Me.HasAura(AurasDefine.SpearDrawn))
            {
                LogHelper.Debug("发近战战争神");
                return AstSpellHelper.CastMeleeCard();
            }
            if (Core.Me.HasAura(AurasDefine.EwerDrawn))
            {
                LogHelper.Debug("发远程河流神");
                return AstSpellHelper.CastRangedCard();
            }
            if (Core.Me.HasAura(AurasDefine.SpireDrawn))
            {
                LogHelper.Debug("发远程建筑神");
                return AstSpellHelper.CastRangedCard();
            }

            return null;
        }
    }
}
