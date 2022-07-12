using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Warrior.Ability
{
    public class WarriorAbility_defense : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (Core.Me.CurrentHealthPercent < 15)//死斗判定
                return SpellsDefine.Holmgang;
            
            if (Core.Me.HasAura(AurasDefine.StemTheFlow) || Core.Me.HasAura(AurasDefine.Rampart) || Core.Me.HasAura(AurasDefine.Vengeance))
                //自身拥有原初的血潮，铁壁，复仇时 不释放防御技能
                return SpellsDefine.Upheaval;
            
            if (Core.Me.CurrentHealthPercent < 45)//复仇判定
                if (SpellsDefine.Vengeance.IsUnlock())//复仇学习检测
                    return SpellsDefine.Vengeance;
                else
                    return SpellsDefine.Rampart;//铁壁
            
            if (Core.Me.CurrentHealthPercent < 55)//铁壁判定
                return SpellsDefine.Rampart;
            
            if (Core.Me.CurrentHealthPercent < 75)//原初的血气判定
                if (SpellsDefine.Bloodwhetting.IsUnlock())//原初的血气学习检测
                    return SpellsDefine.Bloodwhetting;
                else
                    return SpellsDefine.RawIntuition;//原初的直觉
            
            return SpellsDefine.Upheaval;
        }
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<WarriorSettings>().WarriorBloodwhetting)
                return -5;
            
            if (!SettingMgr.GetSetting<WarriorSettings>().WarriorRampart)
                return -5;
            
            if (!SettingMgr.GetSetting<WarriorSettings>().WarriorVengeance)
                return -5;
            
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