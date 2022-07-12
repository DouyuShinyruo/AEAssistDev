using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Paladin.GCD
{
    public class PaladinGCD_Ranged : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (SpellsDefine.HolySpirit.IsUnlock())
            {
                if (!MovementManager.IsMoving)
                    if (Core.Me.CurrentMana > DataBinding.Instance.PaladinSettings.ReserveManaPercentage * Core.Me.MaxMana / 100.0 + SpellsDefine.HolySpirit.GetSpellEntity().SpellData.Cost)
                        return SpellsDefine.HolySpirit;
            }

            if (!Paladin_SpellHelper.OutOfAOERange())
                if (SpellsDefine.TotalEclipse.IsUnlock())
                    return PaladinGCD_Base.GetAOE();

            return SpellsDefine.ShieldLob;
        }
        public int Check(SpellEntity lastSpell)
        {
            if (!Paladin_SpellHelper.OutOfMeleeRange())
                return -5;

            spell = GetSpell();

            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}