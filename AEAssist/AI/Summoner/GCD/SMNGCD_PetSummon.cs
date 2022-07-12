using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;
namespace AEAssist.AI.Summoner.GCD
{
    public class SMNGCD_PetSummon : IAIHandler
    {
        uint spell;


        static bool SwiftcastingSlipStream()
        {
            if (!SpellsDefine.Slipstream.IsUnlock())
                return false;
            if (!ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Garuda))
                return false;

            if (!SpellsDefine.Swiftcast.CoolDownInGCDs(3))
                return false;

            if (DataBinding.Instance.SMNSettings.SwiftcastOption == 0)
                return false;

            return true;
        }



        static uint GetSpell()
        {


            if (DataBinding.Instance.SMNSettings.SaveInstantSpells)
            {
                if (ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Ifrit))
                    return SMN_SpellHelper.GetIfrit();
                if (AIRoot.Instance.CloseBurst || !SMNGCD_Aethercharge.GetSpell().CoolDownInGCDs(SMN_SpellHelper.PetRemaining()*4))
                    return 0;
            }

            //if (SwiftcastingSlipStream())
            //    return GetGaruda();

            if (ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Titan))
                return SMN_SpellHelper.GetTitan();
            if (ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Garuda))
                return SMN_SpellHelper.GetGaruda();
            if (ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Ifrit))
                return SMN_SpellHelper.GetIfrit();
            return 0;
        }

        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (spell == 0)
                return -2;
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