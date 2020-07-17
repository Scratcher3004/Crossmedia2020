using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public List<SubWave> subWaves = new List<SubWave>();
    public float timeToThisWave = 10;
    
    [Serializable]
    public class SubWave
    {
        public GameObject enemyType;
        public int enemyAmount = 5;
        public float timeBetweenEnemySpawn = .2f;
        public float timeToNextSubWave = 1;
    }
}
