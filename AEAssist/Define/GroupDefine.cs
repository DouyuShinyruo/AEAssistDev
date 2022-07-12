using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Collections.Generic;

namespace AEAssist.Define
{
    public static class GroupDefine
    {
        // Tanks
        #region Tanks

        private static readonly List<ClassJobType> Tanks = new List<ClassJobType>()
        {
            ClassJobType.Gladiator,
            ClassJobType.Paladin,
            ClassJobType.Marauder,
            ClassJobType.Warrior,
            ClassJobType.DarkKnight,
            ClassJobType.Gunbreaker,
            ClassJobType.BlueMage,
        };

        #endregion

        // Healers

        #region Healers

        private static readonly List<ClassJobType> Healers = new List<ClassJobType>()
        {
            ClassJobType.Arcanist,
            ClassJobType.Scholar,
            ClassJobType.Conjurer,
            ClassJobType.WhiteMage,
            ClassJobType.Astrologian,
            ClassJobType.BlueMage,
            ClassJobType.Sage,
        };

        #endregion

        // DPS
        #region DPS

        private static readonly List<ClassJobType> Dps = new List<ClassJobType>()
        {
            ClassJobType.Archer,
            ClassJobType.Bard,
            ClassJobType.Thaumaturge,
            ClassJobType.BlackMage,
            ClassJobType.Lancer,
            ClassJobType.Dragoon,
            ClassJobType.Pugilist,
            ClassJobType.Monk,
            ClassJobType.Rogue,
            ClassJobType.Ninja,
            ClassJobType.Machinist,
            ClassJobType.RedMage,
            ClassJobType.Samurai,
            ClassJobType.Summoner,
            ClassJobType.Dancer,
            ClassJobType.BlueMage,
            ClassJobType.Reaper,
        };

        #endregion

        // RangedDPs

        #region RangedDps

        private static readonly List<ClassJobType> RangedDps = new List<ClassJobType>()
        {
            ClassJobType.Archer,
            ClassJobType.Bard,
            ClassJobType.Machinist,
            ClassJobType.Dancer,
            ClassJobType.BlueMage,
        };

        #endregion

        // MeleeDps

        #region MeleeDps

        private static readonly List<ClassJobType> MeleeDps = new List<ClassJobType>()
        {
            ClassJobType.Lancer,
            ClassJobType.Dragoon,
            ClassJobType.Pugilist,
            ClassJobType.Monk,
            ClassJobType.Rogue,
            ClassJobType.Ninja,
            ClassJobType.Samurai,
            ClassJobType.BlueMage,
            ClassJobType.Reaper,
        };

        #endregion

        // RangedDps Card

        #region RangedDpsCard

        private static readonly List<ClassJobType> RangedDpsCard = new List<ClassJobType>()
        {
            ClassJobType.Archer,
            ClassJobType.Bard,
            ClassJobType.Machinist,
            ClassJobType.Dancer,
            ClassJobType.Thaumaturge,
            ClassJobType.BlackMage,
            ClassJobType.Machinist,
            ClassJobType.RedMage,
            ClassJobType.Summoner,
            ClassJobType.BlueMage,
        };

        #endregion

        #region Logics

        public static bool IsTank(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && Tanks.Contains(character.CurrentJob);
        }

        public static bool IsHealer(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && Healers.Contains(character.CurrentJob);
        }

        public static bool IsDps(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && Dps.Contains(character.CurrentJob);
        }

        public static bool IsRangedDps(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && RangedDps.Contains(character.CurrentJob);
        }

        public static bool IsBlueMage(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && ClassJobType.BlueMage.Equals(character.CurrentJob);
        }

        public static bool IsBlueMageHealer(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && ClassJobType.BlueMage.Equals(character.CurrentJob) && character.HasAura(AurasDefine.AetherialMimicryHealer);
        }

        public static bool IsBlueMageTank(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && ClassJobType.BlueMage.Equals(character.CurrentJob) && character.HasAura(AurasDefine.AetherialMimicryTank);
        }

        public static bool IsBlueMageDps(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && ClassJobType.BlueMage.Equals(character.CurrentJob) && character.HasAura(AurasDefine.AetherialMimicryDps);
        }

        public static bool IsRangedDpsCard(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && RangedDpsCard.Contains(character.CurrentJob);
        }

        public static bool IsMeleeDps(this GameObject unit)
        {
            var character = unit as Character;
            return character != null && MeleeDps.Contains(character.CurrentJob);
        }


        public static bool OnPvpMap(this LocalPlayer player)
        {
            return WorldManager.InPvP;
        }
        #endregion
    }
}