using System;
using System.Collections;
using System.Collections.Generic;
using ActionGameFramework.Health;
using Core.Health;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : Targetable
{
    public float cooledSpeed = 1;
    public float burnTimeMultiplier = 1;
    public float burnDamageMultiplier = 1;
    public float waterDamage = 1;

    [Header("Attack")]
    public bool dieOnAttack = false;
    public float damageOnAttack = 5;
    public float AttackRange = .15f;
    [Tooltip("How often to attack per second. Only will be used if die on attack is set to false. Use numbers less than one to set the attack rate below zero.")]
    public float attackRate = 1;
    [Tooltip("Will multiply the damage with the given value if the enemy burns. Set to 1 to disable extra damage.")]
    public float burnMultiplier = 2;
    public bool burnAttacked = false;
    public bool forceBurn = false;
    public float attackFlameTime = 5f;
    
    private NavMeshAgent agent;
    private float attackTimeLeft = 0;
    private float defaultSpeed;
    private float coolDuration;
    private float flameDuration;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        defaultSpeed = agent.speed;
    }

    private void Start()
    {
        agent.SetDestination(Destination.singleton.transform.position);
    }

    protected virtual void Update()
    {
        var pos = transform.position;
        if (pos.x > 1000 || pos.x < -1000 || pos.y > 1000 || pos.y < -1000 || pos.z > 1000 || pos.z < -1000)
        {
            Kill();
        }
        
        if (isDead)
        {
            OnDeath();
            return;
        }
        if (!enabled)
            return;
        if (flameDuration > 0)
        {
            flameDuration -= Time.deltaTime / burnTimeMultiplier;

            if (flameDuration > 0)
            {
                TakeDamage(burnDamageMultiplier * Time.deltaTime);
            }
            else
            {
                flameDuration = 0;
            }
        }
        else if (coolDuration > 0)
        {
            coolDuration -= Time.deltaTime;

            if (coolDuration < 0)
            {
                coolDuration = 0;
            }
            else
            {
                TakeDamage(waterDamage*Time.deltaTime);
            }
        }
        agent.speed = coolDuration > 0 ? cooledSpeed : defaultSpeed;
        if (!agent.isOnNavMesh || Vector3.Distance(transform.position, Destination.singleton.transform.position) > AttackRange)
            return;

        if (attackTimeLeft <= 0)
        {
            Destination.singleton.TakeDamage(flameDuration > 0 ? burnMultiplier * damageOnAttack : damageOnAttack);
            if (burnAttacked && flameDuration > 0 || burnAttacked && forceBurn)
            {
                Destination.singleton.Flame(attackFlameTime);
            }

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
    
    public void TakeDamage(float damage)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (damage == 0)
            return;
        
        var output = new HealthChangeInfo
        {
            damageable = configuration, newHealth = configuration.currentHealth, oldHealth = configuration.currentHealth
        };
        
        if (isDead)
        {
            return;
        }
        
        configuration.ChangeHealth(-damage, output);
        configuration.SafelyDoAction("damaged", output);
        if (isDead)
        {
            configuration.SafelyDoAction("died", output);
        }
    }
    
    public void Flame(float time)
    {
        if (flameDuration < time)
            flameDuration = time;
        coolDuration = 0;
    }
    
    public void Cool(float time)
    {
        if (coolDuration < time)
            coolDuration = time;
        flameDuration = 0;
    }
    
    private void OnDeath()
    {
        Destroy(gameObject);
        enabled = false;
    }
}
