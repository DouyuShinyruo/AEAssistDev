using AEAssist.Define;
using AEAssist.Utilities.CombatMessages;
using ff14bot;

namespace AEAssist.AI.Samurai
{
    public class SamuraiCombatMessageStrategy
    {
        public static void RegisterCombatMessages()
        {

            //Highest priority: Don't show anything if we're not in combat
            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(100,
                                          "",
                                          () => !Core.Me.InCombat || !Core.Me.HasTarget));

            // Second priority: Don't show anything if positional requirements are Nulled
            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(200,
                                          "",
                                          "",
                                          () => Core.Me.HasAura(AurasDefine.TrueNorth)));

            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(300,
                    "GEKKO => BEHIND !!!",
                    "/AEAssist;component/Resources/Images/General/ArrowDownHighlighted.png",
                    SamuraiSpellHelper.GekkoPOSCheck)
            );

            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(400,
                    "KASHA => SIDE !!!",
                    "/AEAssist;component/Resources/Images/General/ArrowSidesHighlighted.png",
                    SamuraiSpellHelper.KashaPOSCheck)
            );
        }
    }
}