using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    Collider weaponCollider;
    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
    }

    // we are using an int here because it plays nicely with the Animation controller
    public void EnableHitbox(int value)
    {
        weaponCollider.enabled = value == 1 ? true : false;
    }

    // here is where we see the power of Interfaces
    // all we need to do is check if the collided object has an
    // IHittable interface. we don't care what it is, so long as it has it
    // if it has it, simply call the Hit() method
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IHittable>() != null)
        {
            IHittable toggle = other.GetComponent<IHittable>();
            toggle.Hit(gameObject);
        }
    }
}
