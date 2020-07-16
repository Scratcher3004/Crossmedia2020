using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : Damageable
{
    [Header("Attack")]
    public bool dieOnAttack = false;
    public float damageOnAttack = 5;
    [Tooltip("How often to attack per second. Only will be used iff die on attack is set to false. Use numbers less than one to set the attack rate below zero.")]
    public float attackRate = 1;
    [Tooltip("Will multiply the damage with the given value if the enemy burns. Set to 1 to disable extra damage.")]
    public float burnMultiplier = 2;
    
    private NavMeshAgent agent;
    private float attackTimeLeft = 0;
    
    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.SetDestination(Destination.singleton.transform.position);
    }

    protected override void Update()
    {
        if (!enabled)
            return;
        
        base.Update();
        
        if (!agent.isOnNavMesh || Vector3.Distance(transform.position, Destination.singleton.transform.position) > 0.15)
            return;

        if (attackTimeLeft <= 0)
        {
            Destination.singleton.TakeDamage(flameDuration > 0 ? burnMultiplier * damageOnAttack : damageOnAttack);

            if (dieOnAttack)
            {
                OnDeath();
                return;
            }
            
            attackTimeLeft = 1 / attackRate;
        }
        else
        {
            attackTimeLeft -= Time.deltaTime;
            if (attackTimeLeft < 0)
                attackTimeLeft = 0;
        }
    }
    
    public float GetDistanceToDest()
    {
        if (!enabled || agent.isStopped)
            return 0;

        return agent.remainingDistance;
    }
    
    protected override void OnDeath()
    {
        Destroy(gameObject);
        enabled = false;
    }
}
