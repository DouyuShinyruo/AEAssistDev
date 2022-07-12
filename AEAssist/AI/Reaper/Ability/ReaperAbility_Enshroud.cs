using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.Ability
{
    public class ReaperAbility_Enshroud : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Enshroud.IsReady())
                return -1;
            return ReaperSpellHelper.ReadyToEnshroud();
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.Enshroud.DoAbility()) return SpellsDefine.Enshroud.GetSpellEntity();

            return null;
        }
    }
}