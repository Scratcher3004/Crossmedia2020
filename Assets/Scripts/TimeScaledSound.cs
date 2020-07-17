using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaledSound : MonoBehaviour
{
    private AudioSource source;
    
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        source.pitch = Time.timeScale;
    }
}
