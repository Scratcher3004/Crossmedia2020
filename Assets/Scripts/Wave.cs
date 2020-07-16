using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    [Serializable]
    public class SubWave
    {
        public GameObject enemyType;
        public int enemyAmount = 5;
        public float timeBetweenEnemySpawn;
        public float timeToNextSubWave = 1;
    }
}
