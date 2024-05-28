using Moyba.Bounds;
using Moyba.Camera;
using Moyba.Enemies;
using Moyba.Fx;
using Moyba.Input;
using Moyba.Projectiles;
using Moyba.Ship;
using UnityEngine;

namespace Moyba.Contracts
{
    [DefaultExecutionOrder(-99)]
    public class Omnibus : ContractBase
    {
        private static Omnibus _Instance;

        [SerializeField, Require(typeof(IBoundsManager))] private Object _bounds;
        [SerializeField, Require(typeof(ICameraManager))] private Object _camera;
        [SerializeField, Require(typeof(IEnemyManager))] private Object _enemies;
        [SerializeField, Require(typeof(IFxManager))] private Object _fx;
        [SerializeField, Require(typeof(IInputManager))] private Object _input;
        [SerializeField, Require(typeof(IProjectileManager))] private Object _projectiles;
        [SerializeField, Require(typeof(IShipManager))] private Object _ship;

        public static IBoundsManager Bounds { get; private set; }
        public static ICameraManager Camera { get; private set; }
        public static IEnemyManager Enemies { get; private set; }
        public static IFxManager Fx { get; private set; }
        public static IInputManager Input { get; private set; }
        public static IProjectileManager Projectiles { get; private set; }
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

                Omnibus.Bounds = (IBoundsManager)_bounds;
                Omnibus.Camera = (ICameraManager)_camera;
                Omnibus.Enemies = (IEnemyManager)_enemies;
                Omnibus.Fx = (IFxManager)_fx;
                Omnibus.Input = (IInputManager)_input;
                Omnibus.Projectiles = (IProjectileManager)_projectiles;
                Omnibus.Ship = (IShipManager)_ship;
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _bounds = _LoadOmnibusAsset<IBoundsManager>() as Object;
            _camera = _LoadOmnibusAsset<ICameraManager>() as Object;
            _enemies = _LoadOmnibusAsset<IEnemyManager>() as Object;
            _fx = _LoadOmnibusAsset<IFxManager>() as Object;
            _input = _LoadOmnibusAsset<IInputManager>() as Object;
            _projectiles = _LoadOmnibusAsset<IProjectileManager>() as Object;
            _ship = _LoadOmnibusAsset<IShipManager>() as Object;
        }
#endif
    }
}
