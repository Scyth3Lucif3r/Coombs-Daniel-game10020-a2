using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<bool> OnToggle;

    bool toggleState = false;

    void Awake()
    {
        
        if (OnToggle == null)
        {
            OnToggle = new UnityEvent<bool>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
        toggleState = !toggleState;

        OnToggle.Invoke(toggleState);
        Debug.Log(toggleState);
    }
}
