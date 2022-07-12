using AEAssist.Define;
using System.Threading.Tasks;

namespace AEAssist.Rotations.Core
{
    public interface IRotation
    {
        /// <summary>
        ///     init after job switch
        /// </summary>
        void Init();

        Task<bool> PreCombatBuff();
        /// <summary>
        /// If there is no target to attack, but it is not Stopped.
        /// </summary>
        /// <returns></returns>
        Task<bool> NoTarget();
        SpellEntity GetBaseGCDSpell();
    }
}