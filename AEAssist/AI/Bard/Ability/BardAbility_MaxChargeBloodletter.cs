﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI
{
    public class BardAbility_MaxChargeBloodletter : IAIHandler
    {
        public int Check(SpellData lastSpell)
        {
            if (lastSpell == SpellsDefine.Bloodletter)
                return -1;
            if (!SpellsDefine.Bloodletter.IsReady())
                return -2;
            if (SpellsDefine.Bloodletter.IsMaxChargeReady())
                return 0;
            return -3;
        }

        public async Task<SpellData> Run()
        {
            SpellData spellData = null;
            if (SpellsDefine.RainofDeath.IsReady() && TargetHelper.CheckNeedUseAOE(25, 8, ConstValue.BardAOECount))
            {
                spellData = SpellsDefine.RainofDeath;
                if (await SpellHelper.CastAbility(spellData, Core.Me.CurrentTarget)) return spellData;
            }

            spellData = SpellsDefine.Bloodletter;
            if (await SpellHelper.CastAbility(spellData, Core.Me.CurrentTarget)) return spellData;

            return null;
        }
    }
}