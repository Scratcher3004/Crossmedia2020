using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flamer : MonoBehaviour
{
    private bool active = false;
    
    public float burnTime = 4;
    [Tooltip("How long should the burner stay active")]
    public float burnerDuration = 1.5f;
    
    public void SetActive(bool Value)
    {
        active = Value;
        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), burnerDuration);
    }

    private void Deactivate()
    {
        SetActive(false);
    }
    
    void OnTriggerStay(Collider coll)
    {
        if (active)
        {
            if (coll.transform.GetComponent<Damageable>())
                coll.transform.GetComponent<Damageable>().Flame(burnTime);
        }
    }
}
