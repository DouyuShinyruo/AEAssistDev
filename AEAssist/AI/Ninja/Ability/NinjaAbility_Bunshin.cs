using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja.Ability
{
    public class NinjaAbility_Bunshin : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Bunshin.IsUnlock())
            {
                return -10;
            }

            if (!SpellsDefine.Bunshin.IsReady())
            {
                return -5;
            }

            if (ActionResourceManager.Ninja.NinkiGauge >= 50)
            {
                return 0;
            }

            if (ActionResourceManager.Ninja.NinkiGauge >= 5 &&
                SpellsDefine.Mug.RecentlyUsed())
            {
                return 1;
            }
            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Bunshin.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}