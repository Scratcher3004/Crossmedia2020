using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
                positions.Add(new KeyValuePair<Vector3, Quaternion>(child.position, child.rotation));
                Destroy(child.gameObject);
            }
            
            Reload();
        }
        
        public void Shoot(Transform target)
        {
            if (!Loaded)
                return;
            
            Loaded = false;
            
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.GetComponent<Missile>().Shoot(target);
            }
            
            Invoke(nameof(Reload), reloadTime);
        }
        
        public void Reload()
        {
            Loaded = true;
            foreach (var p in positions)
            {
                Instantiate(missile, p.Key, p.Value, transform);
            }
        }
    }
}
