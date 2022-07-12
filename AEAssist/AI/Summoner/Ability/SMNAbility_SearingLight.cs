using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;
namespace AEAssist.AI.Summoner.Ability
{
    public class SMNAbility_SearingLight : IAIHandler
    {
        uint spell = SpellsDefine.SearingLight;
        bool CheckAethercharge(int timeleft)
        {
            if (SpellsDefine.Aethercharge.IsUnlock() &&
                SpellsDefine.Aethercharge.GetSpellEntity().Cooldown.TotalMilliseconds < timeleft)
                return true;
            return false;
        }
        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;

            if (AIRoot.Instance.CloseBurst)
            {
                return -2;
            }

            if (!AIRoot.Instance.Is2ndAbilityTime())
                return -3;

            var time = AIRoot.Instance.GetGCDDuration() * 0.5f;

            if (!CheckAethercharge((int)time))
                return -4;

            //if (ActionResourceManager.Summoner.TranceTimer > 0)
            //{
            //    return -5;
            //}

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }
}