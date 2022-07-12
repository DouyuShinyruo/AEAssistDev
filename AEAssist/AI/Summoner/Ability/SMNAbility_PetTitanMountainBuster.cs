using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.Ability
{
    public class SMNAbility_PetTitanMountainBuster : IAIHandler
    {
        uint spell = SpellsDefine.MountainBuster;
        public int Check(SpellEntity lastSpell)
        {
            //if (!SMN_SpellHelper.Titan())
            //{
            //    return -3;
            //}
            if (!Core.Me.HasAura(AurasDefine.TitansFavor))
            {
                return -4;
            }
            //if (!spell.IsReady())
            //    return -1;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }
}