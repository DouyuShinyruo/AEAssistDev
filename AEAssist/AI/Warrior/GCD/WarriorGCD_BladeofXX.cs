using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Warrior.GCD
{
    public class WarriorGCD_BldeofXX : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Confiteor:
                    return SpellsDefine.BladeOfFaith;
                case SpellsDefine.BladeOfFaith:
                    return SpellsDefine.BladeOfTruth;
                case SpellsDefine.BladeOfTruth:
                    return SpellsDefine.BladeOfValor;
            }
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.BladeOfFaith.IsUnlock())
                return -2;
            spell = GetSpell();
            if (spell == 0)
                return -3;
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