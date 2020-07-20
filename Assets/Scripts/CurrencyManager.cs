using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    private int money;
    
    void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }

    public void AddMoney(int m)
    {
        money += m;
    }

    public bool Purchase(int cost)
    {
        if (cost > money)
            return false;
        money -= cost;
        return true;
    }
}
