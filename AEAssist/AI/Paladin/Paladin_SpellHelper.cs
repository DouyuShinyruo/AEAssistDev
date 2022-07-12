using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Paladin
{
    public static class Paladin_SpellHelper
    {

        public static bool Debugging { get; set; } = true;

        public static bool FightorFlightInGCD(int GCD =3)
        {
            if (!SpellsDefine.FightorFlight.IsUnlock())
                return false;

            if (Core.Me.HasAura(AurasDefine.FightOrFight))
                return false;

            //If we have requiescat, we are not using fightorflight even it is ready
            if (Core.Me.HasAura(AurasDefine.Requiescat))
                return false;

            if (SpellsDefine.FightorFlight.CoolDownInGCDs(GCD))
                return true;

            return false;
        }

        public static bool OutOfMeleeRange()
        {
            return !Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget, 3);
        }

        //AOE 这个距离判定有点迷
        public static bool OutOfAOERange()
        {
            return Core.Me.Distance(Core.Me.CurrentTarget) > 5 && OutOfMeleeRange();
        }

        public static bool CheckUseAOE(int count = 3)
        {
            if (!DataBinding.Instance.UseAOE)
                return false;

            if (TargetHelper.CheckNeedUseAOE(0, 5, count))
                return true;
            return false;
        }

        public static bool NeedRenewDot()
        {
            if (CheckUseAOE())
                return false;

            var target = Core.Me.CurrentTarget as Character;
            if (target == null)
                return false;
            var dotTime = 0.0;
            target.CharacterAuras.Aggregate((time, aura) => {
                if (aura.CasterId == Core.Player.ObjectId)
                    if (aura.Id == AurasDefine.GoringBlade || aura.Id == AurasDefine.BladeOfValor)
                        if (aura.TimespanLeft.TotalMilliseconds >= 0)
                            dotTime += aura.TimespanLeft.TotalMilliseconds;
                return time;
            });

            return dotTime < 2 * AIRoot.Instance.GetGCDDuration();
        }

        public static int GCDNeededforCombo()
        {
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.FastBlade:
                    return 2;
                case SpellsDefine.RiotBlade:
                    return 1;
                default:
                    return 3;
            }
        }
        public async static Task<bool> CountDownOpener()
        {
            await SpellsDefine.HolySpirit.DoGCD();
            AIRoot.Instance.RecordGCD(SpellsDefine.HolySpirit.GetSpellEntity());
            int time = 1000;

            await Coroutine.Sleep(1000);

            do
            {
                await Coroutine.Sleep(50);
                time += 50;
                ActionManager.DoAction(SpellsDefine.Intervene, Core.Me.CurrentTarget);
            }
            while (SpellsDefine.Intervene.GetSpellEntity().SpellData.MaxCharges == SpellsDefine.Intervene.GetSpellEntity().SpellData.Charges && time < 3000);

            return time >= 3000;
        }

    }
}