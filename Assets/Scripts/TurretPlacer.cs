using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretPlacer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject ghostPrefab;
    public GameObject turretPrefab;
    public LayerMask groundLayer = 0x1;
    public int cost = 5;
    public Color notPurchasable = Color.red;
    
    private GameObject ghostGo;
    private Ghost ghost;
    private bool placingActive = false;
    private Color normal;
    private Image affected;
    
    void Start()
    {
        ghostGo = Instantiate(ghostPrefab);
        ghost = ghostGo.GetComponent<Ghost>();
        ghostGo.SetActive(false);
        normal = GetComponent<Image>().color;
        CurrencyManager.instance.currencyChange += OnCurrChange;
        Debug.Log(CurrencyManager.instance.GetMoney());
        OnCurrChange(5000, 0);
    }

    private void OnCurrChange(int n, int ch)
    {
        GetComponent<Image>().color = CurrencyManager.instance.CanPurchase(cost) ? normal : notPurchasable;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CurrencyManager.instance.CanPurchase(cost))
            return;
        placingActive = true;
        ghostGo.SetActive(true);
        UpdateGhostPosition(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!placingActive)
            return;
        UpdateGhostPosition(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!placingActive)
            return;
        UpdateGhostPosition(eventData);
        if (!ghost.IsPlaceable)
        {
            ghostGo.SetActive(false);
            return;
        }
        CurrencyManager.instance.Purchase(cost);
        Instantiate(turretPrefab, ghostGo.transform.position, quaternion.identity);
        ghostGo.SetActive(false);
    }
    
    private void UpdateGhostPosition(PointerEventData data)
    {
        var ray = Camera.main.ScreenPointToRay(data.position);
        if (Physics.Raycast(ray, out var hit, 90, groundLayer))
            ghostGo.transform.position = hit.point;
    }
}
