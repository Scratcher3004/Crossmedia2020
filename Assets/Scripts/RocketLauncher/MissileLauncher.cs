using System.Collections.Generic;
using System.Linq;
using ActionGameFramework.Health;
using UnityEngine;

namespace RocketLauncher
{
    public class MissileLauncher : Turret
    {
        public MissileBattery[] batteries;
        public bool shootAllBatteriesAtOnce = false;
        public bool attackDifferentEnemies = true;
        
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
                        Shoot(c);
                        break;
                    }
                    cloned.Remove(c);
                }
            }
            else
            {
                foreach (var mb in batteries)
                {
                    Shoot(mb);
                }
            }
        }

        private void Shoot(MissileBattery mb)
        {
            if (!mb.Loaded)
                return;

            if (!attackDifferentEnemies)
                mb.Shoot(new EnemyBase[]{ trackedEnemy });
            else
            {
                var enemies = FindObjectsOfType<EnemyBase>().ToList();

                enemies.RemoveAll(a => Vector3.Distance(a.transform.position, tower.position) > range);
        
                if (enemies.Count < 1)
                    return;
                
                mb.Shoot(enemies.ToArray());
            }
        }
    }
}
