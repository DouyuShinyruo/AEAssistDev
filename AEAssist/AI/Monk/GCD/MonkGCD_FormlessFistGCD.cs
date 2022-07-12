using AEAssist.Define;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Monk.GCD
{
    public class MonkGCD_FormlessFistGCD : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (Core.Me.HasAura(AurasDefine.FormlessFist) || MonkSpellHelper.LastSpellWasNadi())
            {
                return 0;
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var target = Core.Me.CurrentTarget as Character;
            return await MonkSpellHelper.DoOpoOpoGCDS(target);
        }
    }
}