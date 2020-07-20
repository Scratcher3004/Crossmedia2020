using System.Collections;
using System.Collections.Generic;
using Core.Health;
using UnityEngine;

[RequireComponent(typeof(DamageableBehaviour))]
public class CurrencyOnDeath : MonoBehaviour
{
    public int money = 1;
    
    private void Start()
    {
        GetComponent<DamageableBehaviour>().configuration.died += Death;
    }

    private void Death(HealthChangeInfo none)
    {
        CurrencyManager.instance.AddMoney(money);
    }
}
