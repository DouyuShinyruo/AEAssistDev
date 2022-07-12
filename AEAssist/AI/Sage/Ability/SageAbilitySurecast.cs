﻿using AEAssist.Define;
using AEAssist.Helper;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.Ability
{
    public class SageAbilitySurecast : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Surecast.IsReady()) return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.Surecast.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;
        }
    }
}