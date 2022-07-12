using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.RedMage.GCD
{
    public class RedMageGCD_Base : IAIHandler
    {
        uint spell;        

        static public bool RenMageMana()//白 < 黑
        {
            if (ActionResourceManager.RedMage.WhiteMana < ActionResourceManager.RedMage.BlackMana)
                return true;
            else
                return false;
        }
        static public uint GetSpell()
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(5, 5, ConstValue.RedMageAOECount);
            LogHelper.Info($"AOE is {aoeChecker.ToString()}.");
            if (aoeChecker)//判断是否需要AOE
                return GetAOE();
            else
                return GetSingleTarget();
        }

        public static uint GetSingleTarget()
        {
            bool InRange = Core.Me.CanAttackTargetInRange(Core.Me.CurrentTarget, 6);
            var spell = ActionManager.LastSpellId;
            var MyAura = Core.Me.HasMyAura(AurasDefine.Dualcast);
            if (spell == SpellsDefine.Jolt2)
                spell = SpellsDefine.Jolt;
            if (spell == SpellsDefine.VeraeroIII)
                spell = SpellsDefine.Veraero;
            if (spell == SpellsDefine.VerthunderIII)
                spell = SpellsDefine.Verthunder;

            

            if (ActionResourceManager.RedMage.WhiteMana > 50 && ActionResourceManager.RedMage.BlackMana > 50 && InRange)
                return SpellsDefine.Riposte;//回刺

            switch (spell)
            { 
                case SpellsDefine.Riposte://回刺
                    return SpellsDefine.Zwerchhau;//交击斩
                case SpellsDefine.Zwerchhau:
                    return SpellsDefine.Redoublement; //连攻
                case SpellsDefine.EnchantedRedoublement:
                    if (ActionResourceManager.RedMage.WhiteMana > ActionResourceManager.RedMage.BlackMana)
                        return SpellsDefine.Verholy;//赤神圣
                    else
                        return SpellsDefine.Verflare;//赤核爆

                case SpellsDefine.Verholy:
                    return SpellsDefine.Scorch;//焦热
                case SpellsDefine.Verflare:
                    return SpellsDefine.Scorch;//焦热

                case SpellsDefine.Scorch:
                    return SpellsDefine.Resolution;//决断

                case SpellsDefine.Jolt://摇荡+2
                    if (RenMageMana()) 
                        if (MyAura)
                            return SpellsDefine.Veraero;//赤疾风+6
                        else
                            return SpellsDefine.Jolt;
                    else
                        if (MyAura)
                            return SpellsDefine.Verthunder;//赤闪雷+6
                        else
                        return SpellsDefine.Jolt;

                case SpellsDefine.Veraero:
                    if (Core.Me.HasAura(AurasDefine.VerstoneReady))
                        return SpellsDefine.Verstone;//赤飞石+5
                    else 
                        return SpellsDefine.Jolt;

                case SpellsDefine.Verthunder:
                    if (Core.Me.HasAura(AurasDefine.VerfireReady))
                        return SpellsDefine.Verfire;//赤火焰+5
                    else
                        return SpellsDefine.Jolt;
                default:
                    return SpellsDefine.Jolt;
            }
            
        }
        
        public static uint GetAOE()
        {
            var RedMage = ActionResourceManager.RedMage.WhiteMana > ActionResourceManager.RedMage.BlackMana;
            var RW = ActionResourceManager.RedMage.WhiteMana > 20;
            var RB = ActionResourceManager.RedMage.BlackMana > 20;

            

            if (ActionResourceManager.RedMage.WhiteMana > 50 && ActionResourceManager.RedMage.BlackMana > 50)
                return SpellsDefine.Moulinet;

            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Veraero2://赤烈风
                    if (SpellsDefine.Impact.IsReady())
                        return SpellsDefine.Impact;
                    else
                        return SpellsDefine.Scatter;

                case SpellsDefine.Verthunder2:
                    if (SpellsDefine.Impact.IsReady())
                        return SpellsDefine.Impact;
                    else
                        return SpellsDefine.Scatter;

                case SpellsDefine.Scatter://碎散
                    if (RedMage)
                        return SpellsDefine.Verthunder2;//赤震雷
                    else
                        return SpellsDefine.Veraero2;
                case SpellsDefine.Impact://冲击
                    if (RedMage)
                        return SpellsDefine.Verthunder2;//赤震雷
                    else
                        return SpellsDefine.Veraero2;

                case SpellsDefine.Moulinet:
                    if (RW && RB)
                        return SpellsDefine.Moulinet;
                    else
                        if (RedMage)
                            return SpellsDefine.Verholy;//赤神圣
                        else
                            return SpellsDefine.Verflare;//赤核爆
                default:
                    return SpellsDefine.Veraero2;
            }
        }
        public int Check(SpellEntity lastSpell)
        {

            
            spell = GetSpell();
            //LogHelper.Debug("look this：" + spell.ToString());

            if (spell == SpellsDefine.Verholy && !SpellsDefine.Verholy.IsUnlock()) 
                spell = SpellsDefine.Jolt;
            if (spell == SpellsDefine.Verflare && !SpellsDefine.Verflare.IsUnlock()) 
                spell = SpellsDefine.Jolt;

            LogHelper.Info($"TThere next is   {spell.ToString()},lastspell is {ActionManager.LastSpellId.ToString()}.");
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }
    }
}