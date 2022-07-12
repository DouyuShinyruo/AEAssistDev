using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.GCD
{

    public class SMNGCD_TranceBase : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (SMN_SpellHelper.PhoenixTrance() && SpellsDefine.FountainofFire.IsUnlock())
            {
                return SMN_SpellHelper.CheckUseAOE() ? SpellsDefine.BrandofPurgatory : SpellsDefine.FountainofFire;
            }
            if (SpellsDefine.AstralFlare.IsUnlock())
            {
                if (SMN_SpellHelper.CheckUseAOE())
                    return SpellsDefine.AstralFlare;
                else
                    return SpellsDefine.AstralImpulse;
            }
            return SMNGCD_Base.GetSpell();

        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();

            if (!spell.IsReady())
            {
                return -1;
            }

            if (ActionResourceManager.Summoner.TranceTimer <= 0 || SMN_SpellHelper.AnyPet())
            {
                return -3;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}