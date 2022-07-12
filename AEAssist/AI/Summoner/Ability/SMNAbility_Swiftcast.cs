using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.Ability
{
    public class SMNAbility_Swiftcast : IAIHandler
    {

        uint spell = SpellsDefine.Swiftcast;

        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Swiftcast.IsReady())
                return -1;

            if (AIRoot.Instance.CloseBurst)
            {
                return -2;
            }


            if (SMN_SpellHelper.Garuda() && Core.Me.HasAura(AurasDefine.GarudasFavor) && (DataBinding.Instance.SMNSettings.SwiftcastOption == 1 || DataBinding.Instance.SMNSettings.SwiftcastOption == 3))
                return 1;

            if (SMN_SpellHelper.Ifrit() && DataBinding.Instance.SMNSettings.SwiftcastOption > 1)
                return 2;

            //ensure next one will be ready on time
            if (DataBinding.Instance.SMNSettings.SwiftcastOption == 1 && SMN_SpellHelper.Ifrit() && ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Garuda))
                return 3;
            
            return -99;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }
}