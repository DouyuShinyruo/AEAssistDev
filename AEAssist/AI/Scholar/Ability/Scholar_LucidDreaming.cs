using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Scholar.Ability
{
    public class ScholarAbility_LucidDreaming : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            //LogHelper.Info($"{Core.Me.CurrentManaPercent}");
            if (SpellsDefine.LucidDreaming.IsReady() && Core.Me.CurrentManaPercent <= 60)
                return 0;//醒梦
            return -1;
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