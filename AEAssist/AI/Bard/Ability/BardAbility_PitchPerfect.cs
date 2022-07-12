using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_PitchPerfect : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.PitchPerfect.IsReady())
                return -1;
            if (ActionResourceManager.Bard.ActiveSong != ActionResourceManager.Bard.BardSong.WanderersMinuet)
                return -2;

            if (ActionResourceManager.Bard.Repertoire == 0)
                return -3;

            var time = ActionResourceManager.Bard.Timer.TotalMilliseconds;

            if (time < ConstValue.AuraTick)
                return 1;

            if (ActionResourceManager.Bard.Repertoire == 3)
                return 2;

            var lat = SettingMgr.GetSetting<GeneralSettings>().ActionQueueMs;

            // 诗心两层,马上要跳诗心了,九天又转好,两个诗心也打出去
            if (ActionResourceManager.Bard.Repertoire == 2
                && BardSpellHelper.TimeUntilNextPossibleDoTTick() <= lat
                && SpellsDefine.EmpyrealArrow.IsReady())

                return 3;

            if (AEAssist.DataBinding.Instance.FinalBurst) return 4;

            var buffCountInEnd = BardSpellHelper.HasBuffsCountInEnd(3000);
            if (ActionResourceManager.Bard.Repertoire == 2 && buffCountInEnd > 1) return 5;

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.PitchPerfect.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}