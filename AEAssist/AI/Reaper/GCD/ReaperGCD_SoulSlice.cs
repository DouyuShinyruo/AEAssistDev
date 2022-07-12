using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Reaper.GCD
{
    public class ReaperGCD_SoulSlice : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.SoulSlice.IsReady())
                return -1;

            if (ActionResourceManager.Reaper.SoulGauge >= 50)
                return -2;

            if (SpellsDefine.Enshroud.RecentlyUsed() || Core.Me.HasMyAuraWithTimeleft(AurasDefine.Enshrouded))
                return -3;

            if (ActionManager.ComboTimeLeft > 0 && ActionManager.ComboTimeLeft < 10)
                return -4;

            var ret = ReaperSpellHelper.ReadyToEnshroud();
            if (ret >= 0 || ret == -200)
                return -5;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            // 不是满充能,而且准备附体,先把连击状态打掉
            if (!SpellsDefine.SoulSlice.IsMaxChargeReady(0.2f)
                && ReaperSpellHelper.ReadyToEnshroud() >= 0)
            {
                if (TargetHelper.CheckNeedUseAOE(Core.Me.CurrentTarget, 5, 5))
                {
                    if (AIRoot.GetBattleData<ReaperBattleData>().CurrCombo == ReaperComboStages.NightmareScythe)
                        if (await SpellsDefine.NightmareScythe.DoGCD())
                        {
                            AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.SpinningScythe;
                            return SpellsDefine.NightmareScythe.GetSpellEntity();
                        }
                }
                else
                {
                    if (AIRoot.GetBattleData<ReaperBattleData>().CurrCombo == ReaperComboStages.InfernalSlice)
                    {
                        if (await SpellsDefine.InfernalSlice.DoGCD())
                        {
                            AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.Slice;
                            return SpellsDefine.InfernalSlice.GetSpellEntity();
                        }
                    }
                    else if (AIRoot.GetBattleData<ReaperBattleData>().CurrCombo == ReaperComboStages.WaxingSlice)
                    {
                        if (await SpellsDefine.WaxingSlice.DoGCD())
                        {
                            AIRoot.GetBattleData<ReaperBattleData>().CurrCombo = ReaperComboStages.InfernalSlice;
                            return SpellsDefine.WaxingSlice.GetSpellEntity();
                        }
                    }
                }
            }

            var spell = ReaperSpellHelper.CanUseSoulSlice_Scythe(Core.Me.CurrentTarget);
            if (spell == null)
                return null;
            if (await spell.DoGCD()) return spell;

            return null;
        }
    }
}