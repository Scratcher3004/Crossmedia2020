using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
public class Destination : Damageable
{
    public static Destination singleton;

    protected override void Awake()
    {
        base.Awake();
        
        if (singleton == null)
            singleton = this;
    }

    protected override void OnDeath()
    {
        Time.timeScale = 0;
        
        // TODO Game Over
    }
}
