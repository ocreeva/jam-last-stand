using UnityEngine;

namespace Moyba.Contracts
{
    public abstract class TraitBase<TManager> : ContractBase, IBehaviour
        where TManager : ScriptableObject
    {
        [SerializeField] protected TManager _manager;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _manager = _LoadOmnibusAsset<TManager>();
        }
#endif

        protected abstract class TraitStubBase<TTrait> : IBehaviour
            where TTrait : TraitBase<TManager>
        {
            private bool? _enabled;

            public bool enabled
            {
                get => _enabled.GetValueOrDefault(true);
                set => _enabled = value;
            }

            public virtual void TransferControlFrom(TTrait trait)
            {
                _enabled = trait.enabled;

                this.TransferEvents(trait);
            }

            public virtual void TransferControlTo(TTrait trait)
            {
                if (_enabled.HasValue) trait.enabled = _enabled.Value;

                this.TransferEvents(trait);
            }

            protected virtual void TransferEvents(TTrait trait) { }
        }
    }

    public abstract class TraitBase<TEntity, TManager> : ContractBase
        where TEntity : EntityBase<TManager>
        where TManager : ScriptableObject
    {
        [SerializeField] protected TEntity _entity;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _entity = this.GetComponent<TEntity>();
        }
#endif
    }
}
