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
    [Tooltip("Inverts the effect - Removes burning")]
    public bool cool = false;
    
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
            if (cool)
            {
                if (coll.transform.GetComponent<EnemyBase>())
                    coll.transform.GetComponent<EnemyBase>().Cool(burnTime);
            }
            else
            {
                if (coll.transform.GetComponent<EnemyBase>())
                    coll.transform.GetComponent<EnemyBase>().Flame(burnTime);
            }
        }
    }
}
