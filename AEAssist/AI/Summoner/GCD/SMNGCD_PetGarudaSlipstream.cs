using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner.GCD
{
    public class SMNGCD_PetGarudaSlipstream : IAIHandler
    {
        uint spell = SpellsDefine.Slipstream;
        public int Check(SpellEntity lastSpell)
        {
            if (!Core.Me.HasAura(AurasDefine.GarudasFavor))
                return -3;
            if (!spell.IsReady())
                return -1;
            if (!AIRoot.Instance.CloseBurst)
            {
                if (SpellsDefine.Swiftcast.IsReady())
                    return -4;
                if (SpellsDefine.Swiftcast.CoolDownInGCDs(ActionResourceManager.Summoner.ElementalAttunement / 2))
                    return -4;
            }


            //如果必须要读条且正在移动就不使用
            if (!Core.Me.HasAura(AurasDefine.Swiftcast) && MovementManager.IsMoving)
                return -5;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}