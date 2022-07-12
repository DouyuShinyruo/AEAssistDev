using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.GCD
{
    public class BardGCD_ApexArrow : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!AEAssist.DataBinding.Instance.UseApex)
                return -1;

            if (ActionResourceManager.Bard.SoulVoice >= 20 && AEAssist.DataBinding.Instance.FinalBurst) return 2;

            if (ActionResourceManager.Bard.SoulVoice >= 80 &&
                (BardSpellHelper.HasBuffsCount() >= BardSpellHelper.UnlockBuffsCount() || TargetHelper.CheckNeedUseAOE(
                    25,
                    2, ConstValue.BardAOECount)))
                return 3;


            if (ActionResourceManager.Bard.SoulVoice >= SettingMgr.GetSetting<BardSettings>().ApexArrowValue)
            {
                if (SettingMgr.GetSetting<BardSettings>().ApexWaitBuffs)
                {
                    if (BardSpellHelper.Prepare2BurstBuffs())
                        return -4;
                }

                return 2;
            }

            return -2;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.ApexArrow.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}