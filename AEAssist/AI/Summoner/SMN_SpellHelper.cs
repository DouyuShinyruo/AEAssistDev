using AEAssist.AI.Summoner.GCD;
using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Summoner
{
    public static class SMN_SpellHelper
    {

        public static bool Debugging { get; set; } = true;

        public static bool HasCarbuncle()
        {
            if (GameObjectManager.PetObjectId != GameObjectManager.EmptyGameObject)
                return true;
            if (AnyPet())
                return true;
            if (SummonedPetRecently())
                return true;
            if (PetRemaining() != 0)
                return true;
            if (PhoenixTrance())
                return true;
            return false;
        }

        //this is true from the end of bahamut trance to the end of phoenix trance
        public static bool PhoenixTrance()
        {
            return ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Phoenix);
        }

        public static bool NotMovingWhileSavingInstantSpells()
        {
            return DataBinding.Instance.SMNSettings.SaveInstantSpells && (!MovementManager.IsMoving);
        }


        public static bool CheckUseAOE()
        {
            return TargetHelper.CheckNeedUseAOE(25, 5);
        }
        public static bool AnyPet()
        {
            return Titan() || Ifrit() || Garuda();
        }
        public static bool Titan()
        {
            return ActionResourceManager.Summoner.ActivePet == ActionResourceManager.Summoner.ActivePetType.Titan;
        }

        public static bool Ifrit()
        {
            return ActionResourceManager.Summoner.ActivePet == ActionResourceManager.Summoner.ActivePetType.Ifrit;
        }

        public static bool Garuda()
        {
            return ActionResourceManager.Summoner.ActivePet == ActionResourceManager.Summoner.ActivePetType.Garuda;
        }

        public static int PetRemaining()
        {
            var rtn = 0;
            if (ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Titan))
                rtn++;
            if (ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Ifrit))
                rtn++;
            if (ActionResourceManager.Summoner.AvailablePets.HasFlag(ActionResourceManager.Summoner.AvailablePetFlags.Garuda))
                rtn++;
            return rtn;
        }
        public static uint GetIfrit()
        {
            if (SpellsDefine.SummonIfrit2.IsUnlock())
                return SpellsDefine.SummonIfrit2;
            if (SpellsDefine.SummonIfrit.IsUnlock())
                return SpellsDefine.SummonIfrit;
            return SpellsDefine.SummonRuby;
        }

        public static uint GetTitan()
        {
            if (SpellsDefine.SummonTitan2.IsUnlock())
                return SpellsDefine.SummonTitan2;
            if (SpellsDefine.SummonTitan.IsUnlock())
                return SpellsDefine.SummonTitan;
            return SpellsDefine.SummonTopaz;
        }

        public static uint GetGaruda()
        {
            if (SpellsDefine.SummonGaruda2.IsUnlock())
                return SpellsDefine.SummonGaruda2;
            if (SpellsDefine.SummonGaruda.IsUnlock())
                return SpellsDefine.SummonGaruda;
            return SpellsDefine.SummonEmerald;
        }

        public static bool SummonedPetRecently()
        {
            return GetTitan().GetSpellEntity().RecentlyUsed(2500) || GetIfrit().GetSpellEntity().RecentlyUsed(2500) || GetGaruda().GetSpellEntity().RecentlyUsed(2500) || SMNGCD_Aethercharge.GetSpell().RecentlyUsed() || SpellsDefine.SummonPhoenix.GetSpellEntity().RecentlyUsed(2500);
        }

        public static bool WaitForPotion()
        {
            if (!DataBinding.Instance.GeneralSettings.UsePotion)
                return false;
            if (Core.Me.HasMyAura(AurasDefine.Medicated))
                return false;
            if (!PotionHelper.CheckPotion(SettingMgr.GetSetting<GeneralSettings>().IntPotionId))
                return false;
            return true;
        }
        public async static Task<bool> CountDownOpener()
        {
            await SpellsDefine.Ruin3.DoGCD();
            AIRoot.Instance.RecordGCD(SpellsDefine.Ruin3.GetSpellEntity());
            int time = 1000;

            await Coroutine.Sleep(1000);
            if (!SpellsDefine.SearingLight.IsReady() || !HasCarbuncle())
                return false;
            if (DataBinding.Instance.SMNSettings.DelayOpeningBurst)
                return false;
            do
            {
                await Coroutine.Sleep(50);
                time += 50;
                ActionManager.DoAction(SpellsDefine.SearingLight, Core.Me);
            }
            while (SpellsDefine.SearingLight.GetSpellEntity().Cooldown.TotalMilliseconds <= 0 && time < 3000);

            return time >= 3000;
        }

    }
}