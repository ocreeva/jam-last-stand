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

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Appendix, this), "is stubbing a different instance.");

            _manager.Appendix = null;
        }
    }
}
