using UnityEngine;

namespace Moyba.Enemies
{
    public interface IEnemyManager
    {
        Transform Container { get; }

        int EnemyCount { get; }
        int SpawnerCount { get; }

        event ValueEventHandler<int> OnEnemyCountChanged;
        event ValueEventHandler<int> OnSpawnerCountChanged;
    }
}
