using System;
using ActionGameFramework.Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RocketLauncher
{
    public class SimpleRocketLauncher : Turret
    {
        public GameObject rocket;
        public Transform[] firePoints;
        
        protected override void Shoot()
        {
            var firePoint = firePoints[Random.Range(0, firePoints.Length)];
            var position = firePoint.position;
            var r = Instantiate(rocket, position, firePoint.rotation).GetComponent<HomingLinearProjectile>();
            r.SetHomingTarget(trackedEnemy);
            r.FireInDirection(position, firePoint.forward);
        }
    }
}
