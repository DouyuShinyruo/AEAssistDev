using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.Ability
{
    public class DancerAbility_FanDance3 : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.FanDance3.IsUnlock())
            {
                return -10;
            }

            if (!Core.Me.HasAura(AurasDefine.ThreeFoldFanDance))
            {
                return -1;
            }
            //尽量留到爆发期
            //如果有四个叶子 -> 并且有触发proc -> 使用
            //如果没有四个叶子 -> buff快到了 -> 使用 （有可能被跳舞拖到过期）

            // if (SpellsDefine.TechnicalStep.GetSpellEntity().SpellData.Cooldown < TimeSpan.FromSeconds(5))
            // {
            //     if (ActionResourceManager.Dancer.FourFoldFeathers < 4)
            //     {
            //         return -2;
            //     }
            //     else
            //     {
            //         if (Core.Me.HasAura(AurasDefine.FlourishingSymmetry) || Core.Me.HasAura(AurasDefine.FlourshingFlow))
            //         {
            //             if (SpellsDefine.TechnicalStep.CoolDownInGCDs(1))
            //             {
            //                 return -4;
            //             }
            //         }
            //     }
            // }

            if (SpellsDefine.Flourish.RecentlyUsed() && !SpellsDefine.FanDance3.RecentlyUsed())
            {
                return 1;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.FanDance3.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}