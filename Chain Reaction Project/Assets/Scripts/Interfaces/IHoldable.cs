using UnityEngine;

namespace Interfaces
{
    public interface IHoldable
    {
        /// <summary>
        ///     Called when picking up the IHoldable.
        ///     AI will reparent the transform onto itself.
        /// </summary>
        /// <returns>The parent transform of the IHoldable</returns>
        Transform PickUp();

        /// <summary>
        ///     Called when putting down the IHoldable.
        ///     AI will deparent transform from itself.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Whether the position was valid.</returns>
        bool PutDown(Vector3 position);
    }
}