using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretPlacer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject ghostPrefab;
    public GameObject turretPrefab;
    public LayerMask groundLayer = 0x1;
    
    private GameObject ghostGo;
    private Ghost ghost;
    
    void Start()
    {
        ghostGo = Instantiate(ghostPrefab);
        ghost = ghostGo.GetComponent<Ghost>();
        ghostGo.SetActive(false);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        ghostGo.SetActive(true);
        UpdateGhostPosition(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        UpdateGhostPosition(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        UpdateGhostPosition(eventData);
        if (!ghost.IsPlaceable)
        {
            ghostGo.SetActive(false);
            return;
        }
        
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
