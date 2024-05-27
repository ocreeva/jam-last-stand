using System;
using Moyba.Contracts;
using Moyba.Projectiles;
using UnityEngine;

namespace Moyba.Enemies
{
    public class EnemyHealth : TraitBase<Enemy, EnemyManager>, IEnemyHealth, IDamageable
    {
        private const string _DamageLiteral = "Damage";
        private static readonly int _DamageParameter = Animator.StringToHash(_DamageLiteral);

        [Header("Configuration")]
        [SerializeField, Range(1, 100)] private int _maximumHealth = 5;

        [Header("Components")]
        [SerializeField] private Animator _animator;

        [NonSerialized] private int _currentHealth;

        public void ApplyDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0) UnityEngine.Object.Destroy(this.gameObject);
            else _animator.SetTrigger(_DamageParameter);
        }

        private void Awake()
        {
            _currentHealth = _maximumHealth;
        }
    }
}
