using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpecialEnemies
{
    public class Sokrates : MonoBehaviour
    {
        public Vector2 effectTriggerRange = new Vector2(2, 5);
        public float effectLength = 0.5f;
        
        private List<Light> toDim = new List<Light>();
        
        private void Start()
        {
            toDim = FindObjectsOfType<Light>().ToList();
            StartCoroutine(Loop());
        }
        
        private IEnumerator Loop()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(Random.Range(effectTriggerRange.x, effectTriggerRange.y));
                StartCoroutine(Dim());
            }
        }
        
        private IEnumerator Dim()
        {
            var turrets = FindObjectsOfType<Turret>().ToList();
            foreach (var affect in turrets)
            {
                affect.enabled = false;
            }
            foreach (var affect in toDim)
            {
                affect.enabled = false;
            }
            yield return new WaitForSeconds(effectLength);
            foreach (var affect in toDim)
            {
                affect.enabled = true;
            }
            foreach (var affect in turrets)
            {
                affect.enabled = true;
            }
        }
    }
}
