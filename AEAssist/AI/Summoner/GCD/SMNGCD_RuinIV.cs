using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;
namespace AEAssist.AI.Summoner.GCD
{
    public class SMNGCD_RuinIV : IAIHandler
    {
        uint spell = SpellsDefine.Ruin4;

        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;
            if ((AIRoot.Instance.CloseBurst || SMN_SpellHelper.NotMovingWhileSavingInstantSpells()) && !SpellsDefine.EnergyDrain.CoolDownInGCDs(2))
            {

                return -2;
            }
            if (!Core.Me.HasAura(AurasDefine.FurtherRuin))
                return -10;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}