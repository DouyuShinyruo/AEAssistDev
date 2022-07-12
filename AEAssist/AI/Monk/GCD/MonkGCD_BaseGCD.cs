using AEAssist.Define;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.GCD
{
    public class MonkGCD_BaseGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var target = Core.Me.CurrentTarget as Character;
            return await MonkSpellHelper.BaseGCDCombo(target);
        }
    }
}