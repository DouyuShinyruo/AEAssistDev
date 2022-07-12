using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System;
using System.Threading.Tasks;

namespace AEAssist.AI.BlackMage.Ability
{
    public class BlackMageAblity_Sharpcast : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Sharpcast.IsReady())
            {
                return -1;
            }

            if (SpellsDefine.Sharpcast.RecentlyUsed() ||
                Core.Me.HasAura(AurasDefine.Sharpcast))
            {
                return -2;
            }
            // if we have less then 10 seconds before sharpcast overflow
            if (SpellsDefine.Sharpcast.GetSpellEntity().Cooldown < TimeSpan.FromMilliseconds(10000))
            {

            }

            // we want it to be used on next thunder
            if (!Core.Me.HasAura(AurasDefine.Sharpcast) &&
                SpellsDefine.Sharpcast.GetSpellEntity().SpellData.Charges > 1
                )
            {
                return 1;
            }


            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Sharpcast.GetSpellEntity();
            if (spell == null)
                return null;
            var ret = await spell.DoAbility();
            if (ret)
                return spell;
            return null;
        }
    }
}