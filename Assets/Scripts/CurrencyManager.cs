using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    
    public delegate void OnCurrencyChange(int newCurrency, int currencyChange);
    public OnCurrencyChange currencyChange;
    public int startMoney = 20;
    public Text currencyDisplay;

    private int money;
    
    void Awake()
    {
        money = startMoney;
        currencyChange += OnCurrChange;
        
        if (instance)
            Destroy(this);
        else
            instance = this;
    }

    public void AddMoney(int m)
    {
        money += m;
        currencyChange(money, m);
    }
    
    public bool Purchase(int cost)
    {
        if (cost > money)
            return false;
        money -= cost;
        currencyChange(money, -cost);
        return true;
    }

    public bool CanPurchase(int cost)
    {
        return cost <= money;
    }

    private void OnCurrChange(int curr, int add)
    {
        currencyDisplay.text = money.ToString();
    }

    public int GetMoney()
    {
        return money;
    }
}
