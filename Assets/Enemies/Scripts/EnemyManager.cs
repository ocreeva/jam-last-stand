using System;
using System.Collections.Generic;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Enemies
{
    [CreateAssetMenu(fileName = "Omnibus.Enemies.asset", menuName = "Omnibus/Enemies", order = 1)]
    public class EnemyManager : ScriptableObjectContract, IEnemyManager
    {
        [NonSerialized] private readonly HashSet<IEnemy> _enemies = new HashSet<IEnemy>();
        [NonSerialized] private readonly HashSet<IEnemySpawner> _spawners = new HashSet<IEnemySpawner>();

        public Transform Container => this.Appendix.Container;

        public int EnemyCount => _enemies.Count;
        public int SpawnerCount => _spawners.Count;

        public event ValueEventHandler<int> OnEnemyCountChanged;
        public event ValueEventHandler<int> OnSpawnerCountChanged;

        internal EnemyAppendix Appendix { get; set; }

        internal void Deregister(IEnemy enemy)
        {
            _enemies.Remove(enemy);
            this.OnEnemyCountChanged?.Invoke(this, _enemies.Count);
        }

        internal void Register(IEnemy enemy)
        {
            _enemies.Add(enemy);
            this.OnEnemyCountChanged?.Invoke(this, _enemies.Count);
        }

        internal void Deregister(IEnemySpawner spawner)
        {
            _spawners.Remove(spawner);
            this.OnSpawnerCountChanged?.Invoke(this, _spawners.Count);
        }

        internal void Register(IEnemySpawner spawner)
        {
            _spawners.Add(spawner);
            this.OnSpawnerCountChanged?.Invoke(this, _spawners.Count);
        }
    }
}
