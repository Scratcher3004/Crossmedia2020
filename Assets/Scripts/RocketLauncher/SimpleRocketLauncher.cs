using UnityEngine;

namespace RocketLauncher
{
    public class SimpleRocketLauncher : Turret
    {
        public GameObject rocket;
        [Tooltip("Will be used instead of FirePoint")]
        public Transform[] firePoints;

        protected override void Shoot()
        {
            base.Shoot();
        }
    }
}
