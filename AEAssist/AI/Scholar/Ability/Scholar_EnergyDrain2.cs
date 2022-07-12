using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Scholar.Ability
{
    public class ScholarAbility_EnergyDrain2 : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (ActionResourceManager.Scholar.Aetherflow > 1)//能量吸收判定
                return SpellsDefine.EnergyDrain2;
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (spell == 0) return -1;
            if (!spell.IsReady())
                return -1;
            //LogHelper.Debug("NO10:" + spell.ToString());
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}