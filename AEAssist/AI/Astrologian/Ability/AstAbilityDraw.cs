using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityDraw : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            if (!SpellsDefine.Draw.IsReady()) return -1;

            //if (Core.Me.HasAura(AurasDefine.TheArrow) && Core.Me.HasAura(AurasDefine.TheBalance) && Core.Me.HasAura(AurasDefine.TheSpear) && Core.Me.HasAura(AurasDefine.TheBole) && Core.Me.HasAura(AurasDefine.TheEwer) && Core.Me.HasAura(AurasDefine.TheSpire))
            //{
            //LogHelper.Debug("有卡不抽");
            //return -2;
            //}
            if (ActionResourceManager.Astrologian.Arcana != 0)
            {
                if (!(Core.Me.HasAura(AurasDefine.ArrowDrawn) || Core.Me.HasAura(AurasDefine.BalanceDrawn) || Core.Me.HasAura(AurasDefine.SpearDrawn) || Core.Me.HasAura(AurasDefine.BoleDrawn) || Core.Me.HasAura(AurasDefine.EwerDrawn) || Core.Me.HasAura(AurasDefine.SpireDrawn)))
                {
                    LogHelper.Debug("没卡抽卡");
                    return 0;
                    //AurasDefine.TheBalance;
                }
                LogHelper.Debug("有卡不抽");
                return -2;
            }
            /*
            if (Core.Me.HasAura(AurasDefine.ArrowDrawn) || Core.Me.HasAura(AurasDefine.BalanceDrawn) || Core.Me.HasAura(AurasDefine.SpearDrawn) || Core.Me.HasAura(AurasDefine.BoleDrawn) && Core.Me.HasAura(AurasDefine.EwerDrawn) || Core.Me.HasAura(AurasDefine.SpireDrawn))
            {
                LogHelper.Debug("有卡不抽");
                return -2;
                //AurasDefine.TheBalance;
            }
            if (!(Core.Me.HasAura(AurasDefine.ArrowDrawn) || Core.Me.HasAura(AurasDefine.BalanceDrawn) || Core.Me.HasAura(AurasDefine.SpearDrawn) || Core.Me.HasAura(AurasDefine.BoleDrawn) && Core.Me.HasAura(AurasDefine.EwerDrawn) || Core.Me.HasAura(AurasDefine.SpireDrawn)))
            {
                LogHelper.Debug("没卡抽卡");
                return 0;
                //AurasDefine.TheBalance;
            }
            */
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            LogHelper.Debug("开始");
            LogHelper.Debug(Convert.ToString(SpellsDefine.Divination.GetSpellEntity().Cooldown.TotalSeconds));
            LogHelper.Debug(Convert.ToString(ActionResourceManager.Astrologian.Arcana));
            LogHelper.Debug(Convert.ToString(ActionResourceManager.Astrologian.SealCount));
            LogHelper.Debug(Convert.ToString(ActionResourceManager.Astrologian.UniqueSeals));
            LogHelper.Debug(Convert.ToString(ActionResourceManager.Astrologian.DivinationSeals));
            LogHelper.Debug(Convert.ToString(ActionResourceManager.Astrologian.Timer));
            var spell = SpellsDefine.Draw.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}
