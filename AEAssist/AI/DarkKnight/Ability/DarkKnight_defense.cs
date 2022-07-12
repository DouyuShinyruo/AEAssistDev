using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.DarkKnight.Ability
{
    public class DarkKnightAbility_defense : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            //if (Core.Me.CurrentHealthPercent < 15)//行尸走肉判定
                //return SpellsDefine.LivingDead;
           
            if (Core.Me.CurrentHealthPercent < 45)//暗影墙判定
                if (SpellsDefine.ShadowWall.IsUnlock())
                    return SpellsDefine.ShadowWall;
                else
                    return SpellsDefine.Rampart;//铁壁
            
            if (Core.Me.CurrentHealthPercent < 55)//铁壁判定
                return SpellsDefine.Rampart;
            
            if (Core.Me.CurrentHealthPercent < 75)//至黑之夜判定
                if (SpellsDefine.TheBlackestNight.IsUnlock())
                    return SpellsDefine.TheBlackestNight;
                else
                    return SpellsDefine.BloodWeapon;//随便放了一个嗜血
            
            return SpellsDefine.BloodWeapon;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            
            if (!spell.IsReady())
                return -1;
            //LogHelper.Debug("NO10:" + spell.ToString());
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}