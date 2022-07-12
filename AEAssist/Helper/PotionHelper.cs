using AEAssist.AI;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.Helper
{
    internal class PotionHelper
    {
        public static List<PotionData> DexPotions { get; set; } = new List<PotionData>();
        public static List<PotionData> StrPotions { get; set; } = new List<PotionData>();
        public static List<PotionData> MindPotions { get; set; } = new List<PotionData>();

        public static List<PotionData> IntPotions { get; set; } = new List<PotionData>();

        public static void Init()
        {
            if (DexPotions == null)
                DexPotions = new List<PotionData>();
            DexPotions.Clear();
            DexPotions.Add(new PotionData
            {
                ID = 36110,
                Name = "Grade 6 Dex/6级巧力之幻药"
            });
            DexPotions.Add(new PotionData
            {
                ID = 36105,
                Name = "Grade 5 Dex/5级巧力之幻药"
            });

            DexPotions.Add(new PotionData
            {
                ID = 31894,
                Name = "Grade 4 Dex/4级巧力之幻药"
            });

            DexPotions.Add(new PotionData
            {
                ID = 29493,
                Name = "Grade 3 Dex/3级巧力之幻药"
            });


            if (StrPotions == null)
                StrPotions = new List<PotionData>();
            StrPotions.Add(new PotionData
            {
                ID = 36109,
                Name = "Grade 6 Str/6级刚力之幻药"
            });
            StrPotions.Add(new PotionData
            {
                ID = 36104,
                Name = "Grade 5 Str/5级刚力之幻药"
            });
            StrPotions.Add(new PotionData
            {
                ID = 31893,
                Name = "Grade 4 Str/4级刚力之幻药"
            });
            StrPotions.Add(new PotionData
            {
                ID = 29492,
                Name = "Grade 3 Str/3级刚力之幻药"
            });

            if (MindPotions == null)
                MindPotions = new List<PotionData>();
            MindPotions.Add(new PotionData
            {
                ID = 36113,
                Name = "Grade 6 Mind/6级意力"
            });
            MindPotions.Add(new PotionData
            {
                ID = 36108,
                Name = "Grade 5 Mind/5级意力"
            });
            MindPotions.Add(new PotionData
            {
                ID = 31897,
                Name = "Grade 4 Mind/4级意力"
            });
            MindPotions.Add(new PotionData
            {
                ID = 29496,
                Name = "Grade 3 Mind/3级意力"
            });

            if (IntPotions == null)
                IntPotions = new List<PotionData>();

            IntPotions.Add(new PotionData
            {
                ID = 36112,
                Name = "Grade 6 INT/6级智力"
            });
            IntPotions.Add(new PotionData
            {
                ID = 36107,
                Name = "Grade 5 INT/5级智力"
            });
            IntPotions.Add(new PotionData
            {
                ID = 31896,
                Name = "Grade 4 INT/4级智力"
            });
            IntPotions.Add(new PotionData
            {
                ID = 29495,
                Name = "Grade 3 INT/3级智力"
            });

        }

        public static async Task<bool> UsePotion(int potionRawId)
        {
            if (potionRawId == 0)
                return false;
            if (!SettingMgr.GetSetting<GeneralSettings>().UsePotion)
                return false;
            if (AIRoot.Instance.CloseBurst)
                return false;
            return await ForceUsePotion(potionRawId);
        }

        public static async Task<bool> ForceUsePotion(int potionRawId)
        {
            if (potionRawId == 0)
                return false;
            var item = InventoryManager.FilledSlots.FirstOrDefault(s => s.RawItemId == potionRawId);
            if (item == null)
                return false;
            if (!item.CanUse(Core.Me)) return false;
            LogHelper.Info($@"Using Item >>> {item.Name}");
            for (var i = 0; i < 15; i++)
            {
                item.UseItem(Core.Me);
                await Coroutine.Wait(100, () => false);
                if (item == null || !item.CanUse())
                    return true;
            }

            return false;
        }

        internal static bool CheckPotion(int potionRawId)
        {
            LogHelper.Debug("CheckPotion :" + potionRawId);
            var item = InventoryManager.FilledSlots.FirstOrDefault(s => s.RawItemId == potionRawId);
            if (item == null || !item.CanUse(Core.Me)) return false;

            return true;
        }

        // public static void DebugAllItems()
        // {
        //     foreach (var v in InventoryManager.FilledSlots)
        //     {
        //         LogHelper.Info($"道具 {v.RawItemId} {v.Name} {v.Item.ChnName}");
        //     }
        // }

        public static int CheckNum(int potionId)
        {
            var num = 0;
            foreach (var v in InventoryManager.FilledSlots)
                if (v.RawItemId == potionId)
                    num = (int)(num + v.Item.ItemCount());

            return num;
        }
    }
}