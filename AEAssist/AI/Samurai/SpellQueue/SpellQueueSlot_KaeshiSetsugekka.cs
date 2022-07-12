using AEAssist.Define;
using AEAssist.Helper;

namespace AEAssist.AI.Samurai.SpellQueue
{
    public class SpellQueueSlot_KaeshiSetsugekka : IAISpellQueueSlot
    {
        public int Check(int index)
        {
            return 0;
        }

        public void Fill(SpellQueueSlot slot)
        {
            slot.Abilitys.Enqueue((SpellsDefine.KaeshiSetsugekka, SpellTargetType.CurrTarget));
            // if (SpellsDefine.KaeshiSetsugekka.GetSpellEntity().SpellData.Charges > 0.99)
            //     slot.SetGCDQueue((SpellsDefine.MidareSetsugekka, SpellTargetType.CurrTarget),
            //         (SpellsDefine.KaeshiSetsugekka, SpellTargetType.CurrTarget));
        }
    }
}