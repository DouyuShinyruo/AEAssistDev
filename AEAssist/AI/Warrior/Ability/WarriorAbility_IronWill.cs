using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Warrior.Ability
{
    public class WarriorAbility_IronWill : IAIHandler
    {
        uint spell = SpellsDefine.IronWill;

        public int Check(SpellEntity lastSpell)
        {
            if (!spell.IsReady())
                return -1;
            if (SettingMgr.GetSetting<WarriorSettings>().IronWill && !Core.Me.HasMyAura(AurasDefine.IronWill))
                return 1;
            if (!SettingMgr.GetSetting<WarriorSettings>().IronWill && Core.Me.HasMyAura(AurasDefine.IronWill))
                return 0;
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}