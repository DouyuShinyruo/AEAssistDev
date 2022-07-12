using AEAssist.Define;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEAssist.Helper
{
    public static class GroupHelper
    {
        public static readonly List<Character> CastableParty = new List<Character>();
        public static readonly List<Character> DeadAllies = new List<Character>();
        public static readonly List<Character> CastableTanks = new List<Character>();
        public static readonly List<Character> CastableHealers = new List<Character>();
        public static readonly List<Character> CastableDps = new List<Character>();
        public static readonly List<Character> CastableAlliesWithin30 = new List<Character>();
        public static readonly List<Character> CastableAlliesWithin25 = new List<Character>();
        public static readonly List<Character> CastableAlliesWithin20 = new List<Character>();
        public static readonly List<Character> CastableAlliesWithin15 = new List<Character>();
        public static readonly List<Character> CastableAlliesWithin12 = new List<Character>();
        public static readonly List<Character> CastableAlliesWithin10 = new List<Character>();

        // create pets helper?

        public static bool InParty => PartyManager.IsInParty;
        public static bool PartyInCombat => Core.Me.InCombat;
        public static bool InActiveDuty => DutyManager.InInstance && DutyHelper.State() == DutyHelper.States.InProgress;
        public static bool InGcInstance => RaptureAtkUnitManager.Controls.Any(r => r.Name == "GcArmyOrder");
        public static bool OnPvpMap => Core.Me.OnPvpMap();

        // Add Allies to the List.
        private static void AddAllyToCastable(Character ally)
        {
            // if party member is dead.
            if (ally.CurrentHealth <= 0 || ally.IsDead)
            {
                DeadAllies.Add(ally);
                return;
            }

            if (ally.IsTank())
                CastableTanks.Add(ally);
            if (ally.IsHealer())
                CastableHealers.Add(ally);
            if (ally.IsDps())
                CastableDps.Add(ally);

            var distance = ally.Distance(Core.Me);
            if (distance <= 30) { CastableAlliesWithin30.Add(ally); }
            if (distance <= 25) { CastableAlliesWithin25.Add(ally); }
            if (distance <= 20) { CastableAlliesWithin20.Add(ally); }
            if (distance <= 15) { CastableAlliesWithin15.Add(ally); }
            if (distance <= 12) { CastableAlliesWithin12.Add(ally); }
            if (distance <= 10) { CastableAlliesWithin10.Add(ally); }
        }

        private static void ClearCastable()
        {
            DeadAllies.Clear();
            CastableTanks.Clear();
            CastableHealers.Clear();
            CastableDps.Clear();
            CastableAlliesWithin30.Clear();
            CastableAlliesWithin25.Clear();
            CastableAlliesWithin20.Clear();
            CastableAlliesWithin15.Clear();
            CastableAlliesWithin12.Clear();
            CastableAlliesWithin10.Clear();
        }

        public static void UpdateAllies(Action extensions = null)
        {
            CastableParty.Clear();
            ClearCastable();

            if (!InParty)
            {
                if (!InGcInstance) return;
                CastableParty.Add(Core.Me);
                foreach (var ally in GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(r => !r.CanAttack))
                {
                    //TODO: Make sure this is working properly and no bugs for viewing Cutscene.
                    if (!ally.IsTargetable || !ally.InLineOfSight() || ally.Icon == PlayerIcon.Viewing_Cutscene)
                        continue;
                    CastableParty.Add(ally);
                }
            }

            foreach (var ally in PartyManager.RawMembers.Select(r => r.BattleCharacter))
            {
                if (ally == null)
                    continue;

                if (!ally.IsTargetable || !ally.InLineOfSight() || ally.Icon == PlayerIcon.Viewing_Cutscene)
                    continue;


                if (WorldManager.InPvP)
                {
                    if (ally.HasAura(AurasDefine.MountedPvp))
                        continue;
                }

                CastableParty.Add(ally);
                AddAllyToCastable(ally);
            }
            extensions?.Invoke();
        }
    }
}