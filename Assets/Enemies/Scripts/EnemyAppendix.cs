using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Enemies
{
    public class EnemyAppendix : TraitBase<EnemyManager>
    {
        public Transform Container => this.transform;

        private void Awake()
        {
            this._Assert(_manager.Appendix == null, "is replacing a non-stub instance.");

            _manager.Appendix = this;
        }

        private void HandleEnemyCountChanged(UnityEngine.Object _, int count)
        {
            if (count != 0) return;
            if (Omnibus.Enemies.SpawnerCount != 0) return;

            Omnibus.Instance.LoadPlanetDefenseScene();
        }

        private void HandleSpawnerCountChanged(UnityEngine.Object _, int count)
        {

        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Appendix, this), "is stubbing a different instance.");

            _manager.Appendix = null;
        }

        private void OnDisable()
        {
            _manager.OnEnemyCountChanged -= this.HandleEnemyCountChanged;
            _manager.OnSpawnerCountChanged -= this.HandleSpawnerCountChanged;
        }

        private void OnEnable()
        {
            _manager.OnEnemyCountChanged += this.HandleEnemyCountChanged;
            _manager.OnSpawnerCountChanged += this.HandleSpawnerCountChanged;
        }
    }
}
