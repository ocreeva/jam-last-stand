using Moyba.Bounds;
using Moyba.Camera;
using Moyba.Enemies;
using Moyba.Fx;
using Moyba.Input;
using Moyba.Planet;
using Moyba.Projectiles;
using Moyba.Ship;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField, Require(typeof(IPlanetManager))] private Object _planet;
        [SerializeField, Require(typeof(IProjectileManager))] private Object _projectiles;
        [SerializeField, Require(typeof(IShipManager))] private Object _ship;

        [Header("UI Configuration")]
        [SerializeField] private Color _activeButtonColor = Color.white;
        [SerializeField] private Color _inactiveButtonColor = Color.white;
        [SerializeField] private Color _disabledButtonColor = Color.gray;
        [SerializeField] private Color _healthyStatusColor = Color.green;
        [SerializeField] private Color _damagedStatusColor = Color.yellow;
        [SerializeField] private Color _criticalStatusColor = Color.red;
        [SerializeField] private Color _alertStatusColor = Color.red;
        [SerializeField] private Color _noStatusColor = Color.gray;

        public static Omnibus Instance => _Instance;

        public static IBoundsManager Bounds { get; private set; }
        public static ICameraManager Camera { get; private set; }
        public static IEnemyManager Enemies { get; private set; }
        public static IFxManager Fx { get; private set; }
        public static IInputManager Input { get; private set; }
        public static IPlanetManager Planet { get; private set; }
        public static IProjectileManager Projectiles { get; private set; }
        public static IShipManager Ship { get; private set; }

        public static Color ActiveButtonColor => _Instance._activeButtonColor;
        public static Color InactiveButtonColor => _Instance._inactiveButtonColor;
        public static Color DisabledButtonColor => _Instance._disabledButtonColor;

        public static Color HealthyStatusColor => _Instance._healthyStatusColor;
        public static Color DamagedStatusColor => _Instance._damagedStatusColor;
        public static Color CriticalStatusColor => _Instance._criticalStatusColor;
        public static Color AlertStatusColor => _Instance._alertStatusColor;
        public static Color NoStatusColor => _Instance._noStatusColor;

        public void LoadPlanetDefenseScene()
        {
            SceneManager.LoadScene("Planet Defense");
        }

        public void LoadSpaceCombatScene()
        {
            SceneManager.LoadScene("Space Combat");
        }

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
                Omnibus.Planet = (IPlanetManager)_planet;
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
            _planet = _LoadOmnibusAsset<IPlanetManager>() as Object;
            _projectiles = _LoadOmnibusAsset<IProjectileManager>() as Object;
            _ship = _LoadOmnibusAsset<IShipManager>() as Object;
        }
#endif
    }
}
