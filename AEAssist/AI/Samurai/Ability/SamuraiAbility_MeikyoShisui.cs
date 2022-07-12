using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_MeikyoShisui : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            var needUseAoe = TargetHelper.CheckNeedUseAOE(2, 5);
            if (needUseAoe)
            {
                if (SpellsDefine.MeikyoShisui.IsReady())
                {
                    return 0;
                }
            }
            
            if (!SpellsDefine.MeikyoShisui.IsReady())
                return -14;
            if (Core.Me.HasAura(AurasDefine.MeikyoShisui) || SpellsDefine.MeikyoShisui.RecentlyUsed())
            {
                return -13;
            }
            
            if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.OddMinutesBurstPhase  
                || AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                if (SpellsDefine.MeikyoShisui.GetSpellEntity().SpellData.Charges > 1)
                {
                    return 0;
                }
                
            }
            return -5;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.MeikyoShisui;
            if (await spell.DoAbility())
                return spell.GetSpellEntity();
            return null;
        }
    }
}