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
    protected float flameDuration;
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
    }

    public void TakeDamage(float Value)
    {
        health -= Value;

        if (health <= 0)
        {
            OnDeath();
        }
    }
    
    public void Flame(float Time)
    {
        if (flameDuration < Time)
            flameDuration = Time;
    }

    protected abstract void OnDeath();
}
