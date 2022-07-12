using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.DarkKnight.GCD
{
    public class DarkKnightGCD_Base : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(5, 5, ConstValue.WhiteMageAOECount);       
            
            if (ActionResourceManager.DarkKnight.BlackBlood >= 50 || Core.Me.HasMyAuraWithTimeleft(AurasDefine.Delirium, 1000))
                //暗血量普是否大于等于50 或者 血乱BUFF大于1秒
                return Bloodspiller();//血溅
            else
                if (aoeChecker)//判断是否需要AOE
                    return GetAOE();
                else
                    return GetSingleTarget();
        }
        public static uint Bloodspiller()//血溅
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(5, 8, ConstValue.WhiteMageAOECount);

            if (SpellsDefine.Bloodspiller.IsUnlock())//是否已学习
                return SpellsDefine.Bloodspiller;
            else
                if (aoeChecker)//判断是否需要AOE
                    return SpellsDefine.Bloodspiller;//血溅
                else
                    return SpellsDefine.Quietus;//寂灭
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            LogHelper.Debug("look this：" + spell.ToString());
            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }



        public static uint GetSingleTarget()
        {   
            var tar = Core.Me as Character;
            //LogHelper.Debug(tar.HasMyAuraWithTimeleft(AurasDefine.SurgingTempest, 10000).ToString());
            switch (ActionManager.LastSpellId) 
            {
                case SpellsDefine.HardSlash://重斩
                    return SpellsDefine.SyphonStrike;//吸收斩
                case SpellsDefine.SyphonStrike:
                    if (SpellsDefine.Souleater.IsUnlock())
                        return SpellsDefine.Souleater;//噬魂斩
                    else return SpellsDefine.HardSlash;//重斩

                default:
                    return SpellsDefine.HardSlash;
            }
            
        }

        public static uint GetAOE()
        {
            var tar = Core.Me as Character;
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.Unleash:
                    if (SpellsDefine.StalwartSoul.IsUnlock())
                        return SpellsDefine.StalwartSoul;//刚魂
                    else return SpellsDefine.Unleash;//释放

                default:
                    return SpellsDefine.Unleash;
            }
        }

    }
}