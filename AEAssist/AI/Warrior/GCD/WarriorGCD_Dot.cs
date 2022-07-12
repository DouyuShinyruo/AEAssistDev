using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Warrior.GCD
{
    public class WarriorGCD_Dot : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.FastBlade:
                    return SpellsDefine.RiotBlade;
                case SpellsDefine.RiotBlade:
                    return SpellsDefine.GoringBlade;
                default:
                    return SpellsDefine.FastBlade;
            }


        }
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.GoringBlade.IsUnlock())
                return -2;

            if (!DataBinding.Instance.UseDot)
                return -3;

            if (!Warrior_SpellHelper.NeedRenewDot())
                return -4;
            spell = GetSpell();

            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }


    }
}