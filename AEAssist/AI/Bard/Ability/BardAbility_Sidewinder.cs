using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Bard.Ability
{
    public class BardAbility_Sidewinder : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Sidewinder.IsReady())
                return -1;
            if (BardSpellHelper.HasBuffsCount() >= 2)
                return 1;

            if (BardSpellHelper.CheckCanUseBuffs()) return 2;

            if (!Core.Me.HasMyAura(AurasDefine.RagingStrikes)) return 3;

            if (BardSpellHelper.UnlockBuffsCount() <= 1)
                return 4;

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spellData = SpellsDefine.Sidewinder.GetSpellEntity();
            if (await spellData.DoAbility()) return spellData;

            return null;
        }
    }
}