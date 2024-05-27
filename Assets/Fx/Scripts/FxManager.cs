using UnityEngine;

namespace Moyba.Fx
{
    [CreateAssetMenu(fileName = "Omnibus.Fx.asset", menuName = "Omnibus/Fx", order = 1)]
    public class FxManager : ScriptableObject, IFxManager
    {
        public Transform Container => this.Appendix.Container;

        internal FxAppendix Appendix { get; set; }

        internal void Deregister(IParticleFx particleFx) { }
        internal void Register(IParticleFx particleFx) { }
    }
}
