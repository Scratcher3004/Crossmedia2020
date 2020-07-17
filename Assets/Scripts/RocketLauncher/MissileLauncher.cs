using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RocketLauncher
{
    public class MissileLauncher : Turret
    {
        public MissileBattery[] batteries;
        public bool shootAllBatteriesAtOnce = false;
        
        protected override void Shoot()
        {
            if (!shootAllBatteriesAtOnce)
            {
                var cloned = batteries.ToList();
                
                while (cloned.Count > 0)
                {
                    var c = cloned[Random.Range(0, cloned.Count - 1)];
                    if (c.Loaded)
                    {
                        c.Shoot(trackedEnemy.transform);
                        break;
                    }
                    cloned.Remove(c);
                }
            }
            else
            {
                foreach (var mb in batteries)
                {
                    if (mb.Loaded)
                        mb.Shoot(trackedEnemy.transform);
                }
            }
        }
    }
}
