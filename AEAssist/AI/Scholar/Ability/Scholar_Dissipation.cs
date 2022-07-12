using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Scholar.Ability
{
    public class ScholarAbility_Dissipation : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (GameObjectManager.PetObjectId == GameObjectManager.EmptyGameObject) return 0;
            if (SpellsDefine.Dissipation.IsReady() && ActionResourceManager.Scholar.Aetherflow == 0)//转化判定
                    return SpellsDefine.Dissipation;
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (spell == 0) return -1;
            if (!spell.IsReady())
                return -1;
            //LogHelper.Debug("NO10:" + spell.ToString());
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Dissipation.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }

}