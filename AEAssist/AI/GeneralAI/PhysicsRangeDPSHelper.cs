using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.GeneralAI
{
    public static class PhysicsRangeDPSHelper
    {
        public static async Task<bool> UsePoleton()
        {
            if (PartyManager.IsInParty)
                if (TargetMgr.Instance.EnemysIn25.Count > 0)
                    return false;

            if (Core.Me.HasTarget && Core.Me.CurrentTarget.CanAttack)
                return false;

            if (Core.Me.ContainAura(AurasDefine.Peloton, 100))
                return false;
            if (await SpellsDefine.Peloton.DoAbility())
            {
                GUIHelper.ShowInfo(Language.Instance.Content_Bard_PreCombat3);
                return true;
            }

            return false;
        }
    }
}