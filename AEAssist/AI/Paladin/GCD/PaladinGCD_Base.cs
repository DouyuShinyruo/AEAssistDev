﻿using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
namespace AEAssist.AI.Paladin.GCD
{
    public class PaladinGCD_Base : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (CheckAOE())
                return GetAOE();
            return GetSingleTarget();
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();

            if (!spell.IsReady())
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }

        public static bool CheckAOE()
        {
            if (TargetHelper.CheckNeedUseAOE(0, 5) && SpellsDefine.TotalEclipse.IsUnlock())
                return true;
            return false;
        }

        public static uint GetSingleTarget() 
        {
            if (Core.Me.HasAura(AurasDefine.SwordOath))
                return SpellsDefine.Atonement;

            var lastGCDSpell = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            if (lastGCDSpell == null)
                return SpellsDefine.FastBlade;

            if (lastGCDSpell.Id == SpellsDefine.FastBlade && SpellsDefine.RiotBlade.IsUnlock())
                return SpellsDefine.RiotBlade;

            if (lastGCDSpell.Id == SpellsDefine.RiotBlade && SpellsDefine.RageofHalone.IsUnlock())
                return GetRoyalAuthority();

            return SpellsDefine.FastBlade; 
        }

        public static uint GetRoyalAuthority() 
        {
            if (SpellsDefine.RoyalAuthority.IsUnlock())
                return SpellsDefine.RoyalAuthority;
            return SpellsDefine.RageofHalone;
        }
        public static uint GetAOE() 
        {
            var lastGCDSpell = AIRoot.GetBattleData<BattleData>().lastGCDSpell;
            if (lastGCDSpell == null)
                return SpellsDefine.TotalEclipse;
            
            if (lastGCDSpell.Id == SpellsDefine.TotalEclipse && SpellsDefine.Prominance.IsUnlock())
                return SpellsDefine.Prominance;
            
            return SpellsDefine.TotalEclipse;
            
        }
        
    }
}