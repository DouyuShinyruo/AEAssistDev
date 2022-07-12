using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.GCD
{
    public class SMNGCD_Aethercharge : IAIHandler
    {
        uint spell;

        static public uint GetSpell()
        {
            if (SMN_SpellHelper.PhoenixTrance() && SpellsDefine.SummonPhoenix.IsUnlock())
                return SpellsDefine.SummonPhoenix;
            if (SpellsDefine.SummonBahamut.IsUnlock())
                return SpellsDefine.SummonBahamut;
            if (SpellsDefine.DreadwyrmTrance.IsUnlock())
                return SpellsDefine.DreadwyrmTrance;
            return SpellsDefine.Aethercharge;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();

            if (!spell.IsReady())
                return -1;

            if (AIRoot.Instance.CloseBurst)
                return -2;

            //等一下灼热之光
            if (SpellsDefine.SearingLight.IsUnlock() && SpellsDefine.SearingLight.CoolDownInGCDs(2))
                return -3;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}