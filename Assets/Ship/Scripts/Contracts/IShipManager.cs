using UnityEngine;

namespace Moyba.Ship
{
    public interface IShipManager
    {
        IShipMovement Movement { get; }

        Vector3 Position { get; }
    }
}
