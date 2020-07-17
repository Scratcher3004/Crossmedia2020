using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    private float health;
    [Header("Damageable")]
    public float maxHealth = 100;
    public float flameDamageMultiplier = 5;
    public float flameDurationMultiplier = 1;
    public GameObject deathParticles;
    public Vector3 deathParticlesOffset;
    protected float flameDuration;
    protected float coolDuration;
    public float waterDamage = 0;
    public float GetCurrentHealth => health;

    protected virtual void Awake()
    {
        health = maxHealth;
    }
    
    protected virtual void Update()
    {
        if (flameDuration > 0)
        {
            flameDuration -= Time.deltaTime / flameDurationMultiplier;

            if (flameDuration > 0)
            {
                TakeDamage(flameDamageMultiplier * Time.deltaTime);
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
    }

    public void TakeDamage(float Value)
    {
        health -= Value;

        if (health <= 0)
        {
            if (deathParticles)
            {
                var ins = Instantiate(deathParticles, transform.position + deathParticlesOffset, Quaternion.identity);
                Destroy(ins, ins.GetComponent<ParticleSystem>().main.duration);
            }
            OnDeath();
        }
    }
    
    public void Flame(float Time)
    {
        if (flameDuration < Time)
        {
            flameDuration = Time;
        }
        coolDuration = 0;
    }

    public void Cool(float duration)
    {
        if (coolDuration < duration)
        {
            coolDuration = duration;
        }
        flameDuration = 0;
    }

    protected abstract void OnDeath();
}
