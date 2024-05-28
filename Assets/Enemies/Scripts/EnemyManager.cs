using UnityEngine;

namespace Moyba.Enemies
{
    [CreateAssetMenu(fileName = "Omnibus.Enemies.asset", menuName = "Omnibus/Enemies", order = 1)]
    public class EnemyManager : ScriptableObject, IEnemyManager
    {
        public Transform Container => this.Appendix.Container;

        internal EnemyAppendix Appendix { get; set; }

        internal void Deregister(IEnemy enemy) { }
        internal void Register(IEnemy enemy) { }
    }
}