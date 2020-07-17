using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Color placeable = new Color(0, 1, 0, 0.5f);
    public Color error = new Color(1, 0, 0, 0.5f);
    public Vector3 size = new Vector3(1.5f, 1.5f, 1.5f);
    public LayerMask placedLayer = 0x100;
    
    private Renderer[] renderers;
    private Material shared;
    
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        shared = renderers[0].material;
        
        foreach (var r in renderers)
        {
            r.sharedMaterial = shared;
        }
    }
    
    void Update()
    {
        shared.color = IsPlaceable ? placeable : error;
    }
    
    public bool IsPlaceable => !Physics.CheckBox(transform.position + new Vector3(0, size.y / 2, 0), size / 2, Quaternion.identity,
        placedLayer);
}
