using UnityEngine;

namespace Moyba.Input
{
    [CreateAssetMenu(fileName = "Omnibus.Input.asset", menuName = "Omnibus/Input", order = 1)]
    public class InputManager : ScriptableObject, IInputManager
    {
        public IShipInput Ship { get; internal set; } = ShipInput.Stub;

        internal Controls Controls { get; private set; }

        private void OnEnable()
        {
            if (this.Controls == null) this.Controls = new Controls();
        }
    }
}
