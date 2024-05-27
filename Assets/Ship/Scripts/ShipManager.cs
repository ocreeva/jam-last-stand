using UnityEngine;

namespace Moyba.Ship
{
    [CreateAssetMenu(fileName = "Omnibus.Ship.asset", menuName = "Omnibus/Ship", order = 1)]
    public class ShipManager : ScriptableObject, IShipManager
    {
        public IShipMovement Movement { get; internal set; } = ShipMovement.Stub;

        public Vector3 Position => this.Appendix.Position;

        internal ShipAppendix Appendix { get; set; }
    }
}
