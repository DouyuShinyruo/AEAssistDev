using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.GunBreaker.GCD
{
    public class GunBreakerGCD_BurstStrike : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (ActionResourceManager.Gunbreaker.Cartridge == 0)
                return -5;
            if (TargetHelper.CheckNeedUseAOEByMe(5, 5, 2))
                return -4;
            //子弹将要溢出
            if (ActionManager.LastSpell.Id == SpellsDefine.BrutalShell)
            {
                if (Core.Me.ClassLevel < 88 && ActionResourceManager.Gunbreaker.Cartridge == 2)
                    return 1;
                else if (ActionResourceManager.Gunbreaker.Cartridge == 3)
                    return 2;
            }

            if (SpellsDefine.NoMercy.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds < 5000)
                return -7;

            //在无情中 泻子弹
            if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.NoMercy))
            {
                if (Core.Me.ClassLevel < 90)
                    return 5;
                if (SpellsDefine.DoubleDown.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds == 0 && ActionResourceManager.Gunbreaker.Cartridge > 1)
                    return -1;
                //90级技能冷却时间<无情时间
                else if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.NoMercy, (int)SpellsDefine.DoubleDown.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds)
                    && ActionResourceManager.Gunbreaker.Cartridge > 1)
                    return -51;
                else return 6;
            }

            if (SpellsDefine.Bloodfest.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds < 5000 &&
                ActionResourceManager.Gunbreaker.Cartridge > 0)
            {
                if (ActionResourceManager.Gunbreaker.Cartridge == 1 && SpellsDefine.GnashingFang.GetSpellEntity().SpellData.Cooldown.TotalMilliseconds < 5000)
                    return -2;
                return 7;
            }
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.BurstStrike.GetSpellEntity();
            if (await spell.DoGCD())
                return spell;
            return null;
        }
    }
}