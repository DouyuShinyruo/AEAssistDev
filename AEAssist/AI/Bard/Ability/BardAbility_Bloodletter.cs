using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_Bloodletter : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (lastSpell == SpellsDefine.Bloodletter.GetSpellEntity())
                return -1;
            if (!SpellsDefine.Bloodletter.IsReady())
                return -2;
            if (!AEAssist.DataBinding.Instance.Bloodletter) return -3;

            if (AEAssist.DataBinding.Instance.FinalBurst) return 2;

            if (AIRoot.Instance.CloseBurst)
                return 3;


            if (BardSpellHelper.HasBuffsCount() >= BardSpellHelper.UnlockBuffsCount())
                return 4;

            if (BardSpellHelper.Prepare2BurstBuffs())
                return -4;

            if (Core.Me.ClassLevel >= 84 &&
                ActionResourceManager.Bard.ActiveSong == ActionResourceManager.Bard.BardSong.ArmysPaeon)
                return -5;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var SpellEntity = SpellsDefine.Bloodletter.GetSpellEntity();
            if (SpellsDefine.RainofDeath.IsReady() &&
                TargetHelper.CheckNeedUseAOE(25, 8, ConstValue.BardAOECount))
                SpellEntity = SpellsDefine.RainofDeath.GetSpellEntity();

            if (await SpellEntity.DoAbility()) return SpellEntity;

            return null;
        }
    }
}