using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.GCD
{
    public class GunBreakerGCD_SecondaryCombo : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Gunbreaker.SecondaryComboStage > 0)
            {
                if (DataBinding.Instance.Burst)
                {
                    if (SpellsDefine.SonicBreak.IsReady() || (SpellsDefine.DoubleDown.IsReady() && (ActionResourceManager.Gunbreaker.Cartridge > 1)))
                        return -10;
                    if (SpellsDefine.Bloodfest.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds < 2000 && ActionResourceManager.Gunbreaker.Cartridge > 0)
                        return -11;
                }
                return 100;
            }
            if (!DataBinding.Instance.Burst)
                return -100;

            if (ActionResourceManager.Gunbreaker.Cartridge == 0)
                return -1;

            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 4))
                return -4;

            if (!SpellsDefine.GnashingFang.GetSpellEntity().SpellData.IsReady())
                return -2;

            if (SpellsDefine.NoMercy.GetSpellEntity().SpellData.Charges == 1)
                return -6;

            if (SpellsDefine.NoMercy.GetSpellEntity().SpellData.Charges < 0.5)
                return 1;

            if (SpellsDefine.NoMercy.GetSpellEntity().SpellData.CoolDownInGCDs(4) && SpellsDefine.NoMercy.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds != 0)
                return -5;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = GunBreakerSpellHelper.SecondaryCombo();
            if (await spell.DoGCD())
                return spell;
            return null;
        }
    }
}