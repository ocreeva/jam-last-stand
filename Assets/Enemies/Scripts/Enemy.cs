using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Enemies
{
    public class Enemy : EntityBase<EnemyManager>, IEnemy
    {
        private void OnDisable()
        {
            _manager.Deregister(this);
        }

        private void OnEnable()
        {
            _manager.Register(this);
        }
    }
}
