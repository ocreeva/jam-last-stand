using UnityEngine;

namespace Moyba.Projectiles
{
    [CreateAssetMenu(fileName = "Omnibus.Projectiles.asset", menuName = "Omnibus/Projectiles", order = 1)]
    public class ProjectileManager : ScriptableObject, IProjectileManager
    {
        public Transform Container => this.Appendix.Container;

        internal ProjectileAppendix Appendix { get; set; }

        internal void Deregister(IProjectile _) { }
        internal void Register(IProjectile _) { }
    }
}
