using Moyba.Camera;
using Moyba.Input;
using Moyba.Ship;
using UnityEngine;

namespace Moyba.Contracts
{
    [DefaultExecutionOrder(-99)]
    public class Omnibus : ContractBase
    {
        private static Omnibus _Instance;

        [SerializeField, Require(typeof(ICameraManager))] private Object _camera;
        [SerializeField, Require(typeof(IInputManager))] private Object _input;
        [SerializeField, Require(typeof(IShipManager))] private Object _ship;

        public static ICameraManager Camera { get; private set; }
        public static IInputManager Input { get; private set; }
        public static IShipManager Ship { get; private set; }

        private void Awake()
        {
            if (_Instance)
            {
                if (_Instance != this) Object.Destroy(this.gameObject);
            }
            else
            {
                _Instance = this;
                Object.DontDestroyOnLoad(this.gameObject);

                Omnibus.Camera = (ICameraManager)_camera;
                Omnibus.Input = (IInputManager)_input;
                Omnibus.Ship = (IShipManager)_ship;
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _camera = _LoadOmnibusAsset<ICameraManager>() as Object;
            _input = _LoadOmnibusAsset<IInputManager>() as Object;
            _ship = _LoadOmnibusAsset<IShipManager>() as Object;
        }
#endif
    }
}
