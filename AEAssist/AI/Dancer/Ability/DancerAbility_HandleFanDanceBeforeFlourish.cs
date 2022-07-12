using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Dancer.Ability
{
    public class DancerAbility_HandleFanDanceBeforeFlourish : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.FanDance.IsUnlock())
            {
                return -10;
            }

            if (ActionResourceManager.Dancer.FourFoldFeathers < 1)
            {
                return -1;
            }

            if (SpellsDefine.FanDance.RecentlyUsed())
            {
                return -2;
            }

            if (Core.Me.HasAura(AurasDefine.ThreeFoldFanDance) && !SpellsDefine.FanDance3.RecentlyUsed())
            {
                return -3;
            }

            if (SpellsDefine.Flourish.RecentlyUsed())
            {
                return -2;
            }

            if (SpellsDefine.Flourish.CoolDownInGCDs(2))
            {
                if (ActionResourceManager.Dancer.FourFoldFeathers > 3)
                {
                    return 1;
                }
            }


            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.FanDance.GetSpellEntity();
            if (SpellsDefine.FanDance2.IsUnlock())
            {
                if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 2))
                {
                    spell = SpellsDefine.FanDance2.GetSpellEntity();
                }
            }

            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}