using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Scholar.GCD
{
    public class ScholarGCD_Base : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(5, 5, ConstValue.ScholarAOECount);
            if (aoeChecker)//判断是否需要AOE
                return GetAOE();
            else
                return GetSingleTarget();
        }

        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }



        public static uint GetSingleTarget()
        {
            if (MovementManager.IsMoving)
                return SpellsDefine.SchRuin2;//毁坏
            else
            //GCD-毁灭-毁坏-气炎法-魔炎法-死炎法-极炎法
                return SpellsDefine.SchRuin;//毁灭
        }

        public static uint GetAOE()
        {
          if (SpellsDefine.ArtOfWar.IsUnlock())
                return SpellsDefine.ArtOfWar;

          if (!MovementManager.IsMoving)
                return SpellsDefine.SchRuin2;//毁灭
          else
                return SpellsDefine.SchRuin;//毁坏

        }

    }
}