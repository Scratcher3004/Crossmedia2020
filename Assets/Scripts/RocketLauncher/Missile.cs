using System;
using UnityEngine;
using static UnityEngine.Physics;
using Random = UnityEngine.Random;

namespace RocketLauncher
{
    public class Missile : MonoBehaviour
    {
        /// <summary>
        /// The base movement speed of the missile, in units per second. 
        /// </summary>
        public float speed = 15;
        
        /// <summary>
        /// The base rotation speed of the missile, in radians per second. 
        /// </summary>
        public float rotationSpeed = 1000;
        
        /// <summary>
        /// The Radius of the explosion.
        /// </summary>
        public float explosionRadius = 1;
        
        /// <summary>
        /// The Damage of the explosion.
        /// </summary>
        public float explosionDamage = 2;
        
        /// <summary>
        /// The Explosion Particle System.
        /// </summary>
        public GameObject particles;
        
        /// <summary>
        /// The Duration of the particles.
        /// </summary>
        public float particleDuration = 2;
        
        /// <summary>
        /// The destination height used.
        /// </summary>
        public float heightDestination = 5;
        
        /// <summary>
        /// The tolerance of distance till the rocket explodes.
        /// </summary>
        public float detonationDistanceTolerance = .35f;
        
        [SerializeField]
        private Transform target;
        private bool shooting = false;
        private bool reachedTop = false;
        
        /// <summary>
        /// Shoots at the given target. Only executes when the missile wasn't shot already.
        /// </summary>
        /// <param name="newTarget">The target to shoot at. Set to null if a target was already set in the inspector (for cutscenes).</param>
        public void Shoot(Transform newTarget)
        {
            if (shooting)
                return;
            if (newTarget != null)
                target = newTarget;
            
            shooting = true;
        }
        
        private void Update()
        {
            if (!shooting)
            {
                return;
            }
            
            if (target == null)
            {
                Detonate();
                return;
            }
            
            var position = transform.position;
            if (!reachedTop && transform.position.y < heightDestination)
            {
                var targetDirection1 = new Vector3(position.x + Random.Range(-1, 1), heightDestination,
                                           position.z + Random.Range(-1, 1)) - position;
                
                var newDirection1 =
                    Vector3.RotateTowards(transform.forward, targetDirection1, rotationSpeed * Time.deltaTime, 0.0F);
                
                transform.Translate(Vector3.forward * (Time.deltaTime * speed), Space.Self);
                transform.rotation = Quaternion.LookRotation(newDirection1);
                return;
            }
            else if (!reachedTop && transform.position.y >= heightDestination)
            {
                reachedTop = true;
            }
            
            var targetDirection = target.position - transform.position;
 
            var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0F);
 
            transform.Translate(Vector3.forward * (Time.deltaTime * speed), Space.Self);
            transform.rotation = Quaternion.LookRotation(newDirection);
            
            if (Vector3.Distance(transform.position, target.position) < detonationDistanceTolerance)
                Detonate();
        }

        private void Detonate()
        {
            enabled = false;
            if (particles)
            {
                Destroy(Instantiate(particles), particleDuration);
            }
            var coll = new Collider[20];
            if (OverlapSphereNonAlloc(transform.position, explosionRadius, coll) > 0)
            {
                foreach (var c in coll)
                {
                    if (!c || !c.GetComponent<Damageable>())
                        continue;
                    
                    c.GetComponent<Damageable>().TakeDamage(explosionDamage);
                }
            }
            Destroy(gameObject);
        }
    }
}
