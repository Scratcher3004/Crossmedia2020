using System;
using System.Collections.Generic;
using ActionGameFramework.Health;
using ActionGameFramework.Projectiles;
using TowerDefense.Towers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RocketLauncher
{
    public class MissileBattery : MonoBehaviour
    {
        public bool Loaded { get; private set; } = true;
        public GameObject missile;
        [Range(0.1f, 5f)]
        public float reloadTime = 0.5f;
        
        private List<KeyValuePair<Vector3, Quaternion>> positions = new List<KeyValuePair<Vector3, Quaternion>>();
        
        private void Start()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                positions.Add(new KeyValuePair<Vector3, Quaternion>(child.localPosition, child.localRotation));
                Destroy(child.gameObject);
            }
            
            Reload();
        }
        
        public void Shoot(EnemyBase[] targets)
        {
            if (!Loaded || targets.Length == 0)
                return;
            
            Loaded = false;
            
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var proj = child.GetComponent<HomingLinearProjectile>();
                proj.SetHomingTarget(targets[Random.Range(0, targets.Length)]);
                proj.GetComponent<Rigidbody>().isKinematic = false;
                proj.enabled = true;
                proj.GetComponent<SelfDestroyTimer>().enabled = true;
                proj.FireInDirection(proj.transform.position, proj.transform.forward);
            }
            
            Invoke(nameof(Reload), reloadTime);
        }
        
        public void Reload()
        {
            Loaded = true;
            foreach (var p in positions)
            {
                Instantiate(missile, transform.TransformPoint(p.Key),
                        Quaternion.Euler(transform.TransformDirection(p.Value.eulerAngles)), transform).GetComponent<Rigidbody>()
                    .isKinematic = true;
            }
        }
    }
}
