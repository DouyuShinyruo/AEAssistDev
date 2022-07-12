using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
namespace AEAssist.AI.Paladin.Ability
{
    public class PaladinAbility_SpiritsWithin : IAIHandler
    {
        uint spell = SpellsDefine.SpiritsWithin;

        public uint GetSpell()
        {
            if (SpellsDefine.Expiacion.IsUnlock())
                return SpellsDefine.Expiacion;
            return SpellsDefine.SpiritsWithin;
        }
        public int Check(SpellEntity lastSpell)
        {

            if (!spell.IsReady())
                return -1;
            if (AIRoot.Instance.CloseBurst)
                return -2;
            if (Paladin_SpellHelper.FightorFlightInGCD())
                return -3;
            if (Paladin_SpellHelper.OutOfMeleeRange())
                return -4;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }
}