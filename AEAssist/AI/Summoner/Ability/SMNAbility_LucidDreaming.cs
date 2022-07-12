using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.Ability
{
    public class SMNAbility_LucidDreaming : IAIHandler
    {
        uint spell = SpellsDefine.LucidDreaming;

        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;

            if (Core.Me.CurrentManaPercent >= DataBinding.Instance.SMNSettings.LucidDreamingPercentage)
                return -10;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }
}