using UnityEngine;

namespace Moyba.Contracts
{
    public abstract class EntityBase<TManager> : ContractBase
        where TManager : ScriptableObject
    {
        [SerializeField] protected TManager _manager;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            _manager = _LoadOmnibusAsset<TManager>();
        }
#endif
    }
}
