using ff14bot;
using System.Collections.Generic;

namespace AEAssist.AI.Astrologian
{
    internal class AstBattleData : IBattleData
    {
        public readonly Dictionary<uint, bool> lastCombustWithObj = new Dictionary<uint, bool>();
        public bool IsTargetLastCombust()
        {
            var targetId = Core.Me.CurrentTarget.ObjectId;
            lastCombustWithObj.TryGetValue(targetId, out var ret);
            return ret;
        }
        public int AstNum;
        public bool half = true;
    }
}
