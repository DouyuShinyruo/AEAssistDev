using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Paladin.GCD
{
    public class PaladinGCD_HolySpirit : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if ((LastStack() || LastGCD()) && SpellsDefine.Confiteor.IsUnlock())
                return SpellsDefine.Confiteor;
            if (Paladin_SpellHelper.CheckUseAOE(2) && SpellsDefine.HolyCircle.IsUnlock())
                return SpellsDefine.HolyCircle;
            return SpellsDefine.HolySpirit;
        }
        public int Check(SpellEntity lastSpell)
        {
            if (!Core.Me.HasAura(AurasDefine.Requiescat))
                return -5;

            spell = GetSpell();
            if (!spell.IsReady())
                return -1;

            //Prevent queue stuck here when insufficient mana
            if (Core.Me.CurrentMana < SpellsDefine.HolySpirit.GetSpellEntity().SpellData.Cost)
                return -2;

            //超出距离只能打远程
            if (Paladin_SpellHelper.OutOfMeleeRange())
                return 2;

            //一个gcd都不够了的情况下必须把悔罪打掉
            if (LastGCD() && SpellsDefine.Confiteor.IsUnlock())
            {
                LogHelper.Info($"Requiescat {Core.Me.GetAuraById(AurasDefine.Requiescat).TimespanLeft.TotalMilliseconds} ms left while GCD is {AIRoot.Instance.GetGCDDuration()}. ");
                return 3;
            }
            //如果差一下就可以打出第三段近战 先打第三段
            if (Paladin_SpellHelper.GCDNeededforCombo() == 1)
                return -6;

            //如果有战逃反应 先打物理
            if (Core.Me.HasAura(AurasDefine.FightOrFight))
            {
                if (Core.Me.GetAuraById(AurasDefine.Requiescat).TimespanLeft.TotalMilliseconds < Core.Me.GetAuraById(AurasDefine.Requiescat).Value * AIRoot.Instance.GetGCDDuration())
                    //AOE的情况下  就算有站逃反应 物理伤害也打不过膜法AOE
                    if (spell == SpellsDefine.HolyCircle)
                        return 5;


                //如果站逃只够打一个gcd了 如果打的是先锋剑就不打了
                if (!Core.Me.HasMyAuraWithTimeleft(AurasDefine.FightOrFight, (int)AIRoot.Instance.GetGCDDuration()))
                    if (!Core.Me.HasAura(AurasDefine.SwordOath) && ActionManager.ComboTimeLeft == 0)
                        return 4;

                return -7;
            }

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }

        static bool LastStack()
        {

            return Core.Me.GetAuraById(AurasDefine.Requiescat).Value == 1;

        }
        static bool LastGCD()
        {
            // This would be -30000 at the moment the buff is given, maybe RB bug
            if (Core.Me.GetAuraById(AurasDefine.Requiescat).TimespanLeft.TotalMilliseconds < 0)
                return false;

            return Core.Me.GetAuraById(AurasDefine.Requiescat).TimespanLeft.TotalMilliseconds - 100 < AIRoot.Instance.GetGCDDuration();
        }



    }
}