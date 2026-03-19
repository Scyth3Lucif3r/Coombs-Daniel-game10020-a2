using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour, IHittable
{
    [HideInInspector]
    public UnityEvent<Inventory> OnItemCollected;

    public InventoryItem item;

    public void Awake()
    {
        if (OnItemCollected == null) OnItemCollected = new UnityEvent<Inventory>();
    }

    public void Hit(GameObject otherObjectGameObject)
    {
        OnItemCollected.Invoke(this);
    }
}
