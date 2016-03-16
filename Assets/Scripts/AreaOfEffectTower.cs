using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class AreaOfEffectTower : Tower
    {
        public LayerMask EnemyLayerMask;
        public float AreaDamage = 0.1f;
        public float DamageRange = 2f;
        
        protected override void Fire(Enemy target)
        {
            var hitPosition = target.transform.position;
            var enemies = GetEnemiesInRange(hitPosition).ToList();
            foreach (var enemy in enemies)
            {
                var damage = CalculateDamage(hitPosition, enemy.transform.position);
                enemy.SetHit(damage);
            }
        }

        private IEnumerable<Enemy> GetEnemiesInRange(Vector3 position)
        {
            var colliders = Physics.OverlapSphere(position, DamageRange, EnemyLayerMask);
            return colliders
                .Select(hit => hit.gameObject.GetComponentInParent<Enemy>())
                .Where(enemy => enemy != null)
                .ToList();
        }

        private float CalculateDamage(Vector3 center, Vector3 position)
        {
            var offset = (position - center).magnitude;
            var fraction = Mathf.Clamp01(offset/DamageRange);
            var damage = AreaDamage*fraction*fraction;
            return damage;
        }
    }
}
