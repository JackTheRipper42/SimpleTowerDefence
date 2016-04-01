﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Xml;
using UnityEngine;

namespace Assets.Scripts
{
    public class AreaOfEffectTower : Tower<AreaOfEffectTowerInfo>
    {
        public LayerMask EnemyLayerMask;

        public float AreaDamage { get; private set; }
        public float DamageRange { get; private set; }

        public override void Initialize(AreaOfEffectTowerInfo areaOfEffectTowerInfo, Sprite baseSprite, Sprite towerSprite)
        {
            AreaDamage = areaOfEffectTowerInfo.AreaDamage;
            DamageRange = areaOfEffectTowerInfo.DamageRange;
            base.Initialize(areaOfEffectTowerInfo, baseSprite, towerSprite);
        }

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
