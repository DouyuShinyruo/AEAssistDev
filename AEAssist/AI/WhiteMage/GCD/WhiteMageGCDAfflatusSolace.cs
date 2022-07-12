using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.WhiteMage.GCD
{
    internal class WhiteMageGCDAfflatusSolace : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SettingMgr.GetSetting<WhiteMageSettings>().Heal)
            {
                return -5;
            }
            if (ActionResourceManager.WhiteMage.Lily < 1)
            {
                return -3;
            }
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= SettingMgr.GetSetting<WhiteMageSettings>().AfflatusSolaceHp);
            if (skillTarget == null)
            {
                return -2;
            }

            LogHelper.Debug("安慰之心选取目标：" + Convert.ToString(skillTarget));
            //if (!SpellsDefine.Regen.IsReady()) return -1;

            return 0;

        }
        public Task<SpellEntity> Run()
        {
            return WhiteMageSpellHelper.CastAfflatusSolace();
        }
    }
}
