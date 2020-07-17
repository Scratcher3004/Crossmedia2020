using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigitalRuby.PyroParticles;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Base")]
    public float range = 5;
    public float fireRate = 1;
    public float damagePerShot = 1;
    public float rotationSpeed = 360;
    public float towerYOffset = 0;
    public Transform tower;
    public Transform firePoint;
    public ParticleSystem shootParticles;
    [Tooltip("Leave blank for no animation")]
    public string animatorTrigger;
    
    [Header("Flamethrower")]
    public bool isFlamed = false;
    public Flamer flamer;
    public FireBaseScript fire;
    
    [Header("Debug")]
    public bool drawGizmos = true;
    [Tooltip("Only draw the Gizmos when the Turret is selected")]
    public bool drawGizmosOnlyWhenSelected = true;
    
    private EnemyBase trackedEnemy;
    private float timeToFire = 0;
    private Animator anim;
    
    void Start()
    {
        if (animatorTrigger != string.Empty)
        {
            anim = GetComponent<Animator>();
        }
    }
    
    void Update()
    {
        if (timeToFire > 0)
        {
            timeToFire -= Time.deltaTime;
            if (timeToFire < 0)
                timeToFire = 0;
        }
        
        if (trackedEnemy == null || Vector3.Distance(tower.position, trackedEnemy.transform.position) > range)
        {
            trackedEnemy = null;
            SearchForEnemy();
        }
        
        if (!trackedEnemy)
            return;

        var oldEuler = transform.eulerAngles;
        var lTargetDir = trackedEnemy.transform.position - transform.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lTargetDir), rotationSpeed * Time.deltaTime);
        
        if (timeToFire <= 0 && Vector3.Distance(oldEuler, transform.eulerAngles) < rotationSpeed * 0.75)
        {
            timeToFire = 1 / fireRate;
            trackedEnemy.TakeDamage(damagePerShot);
            
            if (shootParticles)
                shootParticles.Play(true);
            if (anim)
                anim.SetTrigger(animatorTrigger);
            
            if (isFlamed)
            {
                flamer.SetActive(true);
                fire.StartEffect();
            }
        }
    }
    
    private Quaternion QuatYOnly(Quaternion qIn, Vector3 otherValues)
    {
        return Quaternion.Euler(otherValues.x, qIn.eulerAngles.y, otherValues.z);
    }
    
    private void SearchForEnemy()
    {
        var enemies = FindObjectsOfType<EnemyBase>().ToList();

        enemies.RemoveAll(a => Vector3.Distance(a.transform.position, tower.position) > range);
        
        if (enemies.Count < 1)
            return;

        if (enemies.Count == 1) 
        {
            trackedEnemy = enemies[0];
            return;
        }
        
        float enemyVal = 9999999;
        foreach (var enemy in enemies)
        {
            if (enemy.GetDistanceToDest() < enemyVal)
            {
                trackedEnemy = enemy;
                enemyVal = enemy.GetDistanceToDest();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmosOnlyWhenSelected || tower == null)
            return;
        
        DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmosOnlyWhenSelected || tower == null)
            return;

        DrawGizmos();
    }

    protected virtual void DrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(tower.position, range);
    }
}
