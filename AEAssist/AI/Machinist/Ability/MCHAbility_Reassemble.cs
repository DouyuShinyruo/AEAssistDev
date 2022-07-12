using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Machinist.Ability
{
    public class MCHAbility_Reassemble : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Reassemble.IsReady())
                return -1;
            if (ActionResourceManager.Machinist.OverheatRemaining.TotalMilliseconds > 0
                && AIRoot.GetBattleData<MCHBattleData>().HyperchargeGCDCount < 5)
                return -10;

            if (!AIRoot.Instance.Is2ndAbilityTime())
                return -3;

            var time = AIRoot.Instance.GetGCDDuration() * 0.5f;

            if (!MCHSpellHelper.CheckReassmableGCD((int)time))
                return -2;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.Reassemble.DoAbility()) return SpellsDefine.Reassemble.GetSpellEntity();

            return null;
        }
    }
}