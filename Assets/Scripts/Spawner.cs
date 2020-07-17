using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public List<Wave> waves;
    public int waveId = 0;
    public Vector2 spawnArea = new Vector2(3, 3);
    private bool waveRunning = false;
    
    void Start()
    {
        if (waves.Count < 1)
        {
            Debug.LogError("No Waves found!");
        }
        StartCoroutine(ExecuteWave(waves[0]));
    }
    
    void Update()
    {
        if (waveRunning || transform.childCount != 0)
            return;
        
        if (waveId == waves.Count - 1)
        {
            Time.timeScale = 0;
            enabled = false;
            // TODO: Win
            return;
        }
        
        waveId++;
        StartCoroutine(ExecuteWave(waves[waveId]));
    }

    IEnumerator ExecuteWave(Wave waveIn)
    {
        waveRunning = true;
        yield return new WaitForSeconds(waveIn.timeToThisWave);
        foreach (var sw in waveIn.subWaves)
        {
            for (var i = 0; i < sw.enemyAmount; i++)
            {
                var e = Instantiate(sw.enemyType,
                    transform.position + new Vector3(Random.Range(-spawnArea.x, spawnArea.x), 0,
                        Random.Range(-spawnArea.y, spawnArea.y)), Quaternion.identity, transform);
                yield return new WaitForSeconds(sw.timeBetweenEnemySpawn);
            }
            yield return new WaitForSeconds(sw.timeToNextSubWave);
        }
        waveRunning = false;
    }
    
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up, new Vector3(spawnArea.x, 2, spawnArea.y));
    }
}
