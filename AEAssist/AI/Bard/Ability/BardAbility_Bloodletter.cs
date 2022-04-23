﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI
{
    public class BardAbility_Bloodletter : IAIHandler
    {
        public int Check(SpellData lastSpell)
        {
            if (lastSpell == SpellsDefine.Bloodletter)
                return -1;
            if (!SpellsDefine.Bloodletter.IsReady())
                return -2;

            if (DataBinding.Instance.FinalBurst)
            {
                return 2;
            }

            if (AIRoot.Instance.CloseBurst)
                return 3;



            if (BardSpellHelper.HasBuffsCount() >= BardSpellHelper.UnlockBuffsCount())
                return 4;
            // 起手爆发期间, 失血箭尽量打进团辅
            if (BardSpellHelper.Prepare2BurstBuffs())
                return -4;

            // 军神期间,小于2.5 不用失血. 但是RB的失血箭次数在最大2层的时候,不准.所以做个保险
            if (Core.Me.ClassLevel >= 84 &&
                ActionResourceManager.Bard.ActiveSong == ActionResourceManager.Bard.BardSong.ArmysPaeon)
                return -5;

            return 0;
        }

        public async Task<SpellData> Run()
        {
            SpellData spellData = null;
            if (SpellsDefine.RainofDeath.IsReady() &&
                TargetHelper.CheckNeedUseAOE(25, 8, ConstValue.BardAOECount))
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