using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using System;
using System.Collections.Generic;

namespace AEAssist.AI.Sage
{
    [Opener(ClassJobType.Sage, 90, "Test")]
    public class OpenerSageTest : IOpener
    {
        public int Check()
        {
            if (PartyManager.NumMembers <= 4 && !Core.Me.CurrentTarget.IsDummy())
                return -5;

            return 0;
        }

        public List<Action<SpellQueueSlot>> Openers { get; } = new List<Action<SpellQueueSlot>>()
        {
            Step0
        };

        private static void Step0(SpellQueueSlot slot)
        {
            slot.SetGCD(SpellsDefine.Eukrasia, SpellTargetType.Self);
        }
    }
}