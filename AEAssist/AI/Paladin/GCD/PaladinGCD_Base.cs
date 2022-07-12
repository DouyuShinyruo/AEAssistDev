using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Paladin.GCD
{
    public class PaladinGCD_Base : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (Paladin_SpellHelper.CheckUseAOE() && SpellsDefine.TotalEclipse.IsUnlock())
                return GetAOE();
            return GetSingleTarget();
        }
        public int Check(SpellEntity lastSpell)
        {
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



        public static uint GetSingleTarget()
        {
            if (Core.Me.HasAura(AurasDefine.SwordOath))
                return SpellsDefine.Atonement;

            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.FastBlade:
                    if (SpellsDefine.RageofHalone.IsUnlock())
                        return SpellsDefine.RiotBlade;
                    else return SpellsDefine.FastBlade;
                case SpellsDefine.RiotBlade:
                    if (SpellsDefine.RageofHalone.IsUnlock())
                        return GetRoyalAuthority();
                    else return SpellsDefine.FastBlade;
                default:
                    return SpellsDefine.FastBlade;
            }

        }

        public static uint GetRoyalAuthority()
        {
            if (SpellsDefine.RoyalAuthority.IsUnlock())
                return SpellsDefine.RoyalAuthority;
            return SpellsDefine.RageofHalone;
        }
        public static uint GetAOE()
        {

            if (ActionManager.LastSpellId == SpellsDefine.TotalEclipse && SpellsDefine.Prominance.IsUnlock())
                return SpellsDefine.Prominance;

            return SpellsDefine.TotalEclipse;

        }

    }
}