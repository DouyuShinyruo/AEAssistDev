using AEAssist.AI.Dancer;
using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Ninja
{
    public class NinjaSpellHelper
    {
        //Action Name:Spinning Edge 双刃旋 Action Id:2240
        //Action Name:Gust Slash 绝风 Action Id:2242
        //Action Name:Assassinate 断绝 Action Id:2246
        //Action Name:Mug 夺取 Action Id:2248
        //Action Name:Death Blossom 血雨飞花 Action Id:2254
        //Action Name:Aeolian Edge 旋风刃 Action Id:2255
        //Action Name:Trick Attack 攻其不备 Action Id:2258
        //Action Name:Ten 天之印 Action Id:2259
        //Action Name:Ninjutsu 忍术 Action Id:2260
        //Action Name:Chi 地之印 Action Id:2261
        //Action Name:Shukuchi 缩地 Action Id:2262
        //Action Name:Jin 人之印 Action Id:2263
        //Action Name:Kassatsu 生杀予夺 Action Id:2264
        //Action Name:Fuma Shuriken 风魔手里剑 Action Id:2265
        //Action Name:Katon 火遁之术 Action Id:2266
        //Action Name:Raiton 雷遁之术 Action Id:2267
        //Action Name:Hyoton 冰遁之术 Action Id:2268
        //Action Name:Huton 风遁之术 Action Id:2269
        //Action Name:Doton 土遁之术 Action Id:2270
        //Action Name:Suiton 水遁之术 Action Id:2271
        //Action Name:Rabbit Medium 通灵之术 Action Id:2272
        //Action Name:Armor Crush 强甲破点突 Action Id:3563
        //Action Name:Dream Within a Dream 梦幻三段 Action Id:3566
        //Action Name:Hellfrog Medium 通灵之术·大虾蟆 Action Id:7401
        //Action Name:Bhavacakra 六道轮回 Action Id:7402
        //Action Name:Ten Chi Jin 天地人 Action Id:7403
        //Action Name:Second Wind 内丹 Action Id:7541
        //Action Name:Bloodbath 浴血 Action Id:7542
        //Action Name:True North 真北 Action Id:7546
        //Action Name:Arm's Length 亲疏自行 Action Id:7548
        //Action Name:Feint 牵制 Action Id:7549
        //Action Name:Leg Sweep 扫腿 Action Id:7863
        //Action Name:Hakke Mujinsatsu 八卦无刃杀 Action Id:16488
        //Action Name:Meisui 命水 Action Id:16489
        //Action Name:Goka Mekkyaku 劫火灭却之术 Action Id:16491
        //Action Name:Hyosho Ranryu 冰晶乱流之术 Action Id:16492
        //Action Name:Bunshin 分身之术 Action Id:16493
        //Action Name:Ten 天之印 Action Id:18805
        //Action Name:Chi 地之印 Action Id:18806
        //Action Name:Jin 人之印 Action Id:18807
        //Action Name:Fuma Shuriken 风魔手里剑 Action Id:18873
        //Action Name:Fuma Shuriken 风魔手里剑 Action Id:18874
        //Action Name:Fuma Shuriken 风魔手里剑 Action Id:18875
        //Action Name:Katon 火遁之术 Action Id:18876
        //Action Name:Raiton 雷遁之术 Action Id:18877
        //Action Name:Hyoton 冰遁之术 Action Id:18878
        //Action Name:Huton 风遁之术 Action Id:18879
        //Action Name:Doton 土遁之术 Action Id:18880
        //Action Name:Suiton 水遁之术 Action Id:18881
        //Action Name:Phantom Kamaitachi 残影镰鼬 Action Id:25774
        //Action Name:Hollow Nozuchi 幻影野槌 Action Id:25776
        //Action Name:Forked Raiju 月影雷兽爪 Action Id:25777
        //Action Name:Fleeting Raiju 月影雷兽牙 Action Id:25778
        //Action Name:Huraijin 风来刃 Action Id:25876
        private static async Task<SpellEntity> UseAOECombo(GameObject target)
        {
            if (AIRoot.GetBattleData<NinjaBattleData>().CurrCombo != NinjaComboStages.HakkeMujinsatsu
                || ActionManager.ComboTimeLeft <= 0)
            {
                if (await SpellsDefine.DeathBlossom.DoGCD())
                {
                    AIRoot.GetBattleData<NinjaBattleData>().CurrCombo = NinjaComboStages.HakkeMujinsatsu;
                    return SpellsDefine.DeathBlossom.GetSpellEntity();
                }
            }
            else if (await SpellsDefine.HakkeMujinsatsu.DoGCD())
            {
                AIRoot.GetBattleData<NinjaBattleData>().CurrCombo = NinjaComboStages.DeathBlossom;
                return SpellsDefine.HakkeMujinsatsu.GetSpellEntity();
            }

            return null;
        }

        private static async Task<SpellEntity> UseSingleCombo(GameObject target)
        {
            if (ActionManager.ComboTimeLeft > 0)
            {
                if (AIRoot.GetBattleData<NinjaBattleData>().CurrCombo == NinjaComboStages.AeolianEdge)
                {
                    //behind 背面
                    if (Core.Me.CurrentTarget.IsBehind)
                    {
                        if (await SpellsDefine.AeolianEdge.DoGCD())
                        {
                            AIRoot.GetBattleData<NinjaBattleData>().CurrCombo = NinjaComboStages.SpinningEdge;
                            return SpellsDefine.AeolianEdge.GetSpellEntity();
                        }
                    }
                    //side 侧面
                    if (Core.Me.CurrentTarget.IsFlanking)
                    {
                        if (await SpellsDefine.ArmorCrush.DoGCD())
                        {
                            AIRoot.GetBattleData<NinjaBattleData>().CurrCombo = NinjaComboStages.SpinningEdge;
                            return SpellsDefine.ArmorCrush.GetSpellEntity();
                        }
                    }
                }
                else if (AIRoot.GetBattleData<NinjaBattleData>().CurrCombo == NinjaComboStages.GustSlash)
                {
                    if (await SpellsDefine.GustSlash.DoGCD())
                    {
                        AIRoot.GetBattleData<NinjaBattleData>().CurrCombo = NinjaComboStages.AeolianEdge;
                        return SpellsDefine.GustSlash.GetSpellEntity();
                    }
                }
            }

            if (await SpellsDefine.SpinningEdge.DoGCD())
            {
                AIRoot.GetBattleData<NinjaBattleData>().CurrCombo = NinjaComboStages.GustSlash;
                return SpellsDefine.SpinningEdge.GetSpellEntity();
            }

            return null;
        }

        public static async Task<SpellEntity> BaseGCDCombo(GameObject target)
        {
            if (TargetHelper.CheckNeedUseAOE(target, 5, 5)) return await UseAOECombo(target);

            return await UseSingleCombo(target);
        }

        public static int NinjutsuCheck()
        {
            var bdls = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            // if we started cast Ninjutsu
            if (Core.Me.HasAura(AurasDefine.Mudra))
            {
                //TODO: get musked spell, check it's not rabbi thing
                return 0;
            }
            // if we didn't start cast Ninjutsu
            if (bdls != SpellsDefine.Ten.GetSpellEntity() &&
                bdls != SpellsDefine.Chi.GetSpellEntity() &&
                bdls != SpellsDefine.Jin.GetSpellEntity())
            {
                return 0;
            }
            return -4;
        }


        public static SpellEntity GetBhavacakra(GameObject target)
        {
            //should be TargetHelper.CheckNeedUseAOE(target, 25, 6, 4) in 6.1
            if (TargetHelper.CheckNeedUseAOE(target, 25, 6, 3)) return SpellsDefine.HellfrogMedium.GetSpellEntity();

            return SpellsDefine.Bhavacakra.GetSpellEntity();
        }


        public static async Task PreCombatDanceSteps()
        {
            bool finish = false;
            const int retryInterval = 100; // 25* 100 = GCD CoolDown
            while (!finish)
            {
                try
                {
                    if (ActionResourceManager.Dancer.Steps.Length > 2)
                    {
                        var steps = ActionResourceManager.Dancer.Steps;
                        foreach (var step in steps)
                        {
                            if (step == ActionResourceManager.Dancer.DanceStep.Finish)
                            {
                                finish = true;
                                break;
                            }
                            var spell = DancerSpellHelper.GetDanceStep(step);
                            while (!await spell.DoGCD())
                            {
                                await Coroutine.Sleep(retryInterval);
                            }

                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

        }

    }
}