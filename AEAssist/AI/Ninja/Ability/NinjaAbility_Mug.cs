using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.Ability
{
    public class NinjaAbility_Mug : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Mug.IsUnlock())
            {
                return -10;
            }
            if (!SpellsDefine.Mug.IsReady())
            {
                return -5;
            }
            if (ActionResourceManager.Ninja.NinkiGauge < 60)
            {
                return 0;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Mug.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}