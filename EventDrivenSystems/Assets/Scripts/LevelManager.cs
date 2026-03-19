using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    public UIManager uiManager;
    public InventoryManager inventoryManager;

    [Header("Character Controller")]
    public Character character;

    [Header("Gameplay Objects")]
    public GameObject barriers1;
    //public Door door;
    public GameObject inventoryItems;
    public PressurePlate pressurePlate;

    [Header("Prefabs")]
    public Inventory weightPrefab;

    void Start()
    {
        inventoryManager.OnInventoryChanged.AddListener(uiManager.UpdateInventoryUI);
        inventoryManager.OnInventorySpawned.AddListener(SpawnInventory);

        foreach (Transform child in inventoryItems.transform)
        {
            Inventory inventory = child.GetComponent<Inventory>();
            inventory.OnItemCollected.AddListener(inventoryManager.PickUpInventory);
        }

        // connect gameplay system events
        foreach (Transform child in barriers1.transform)
        {
            
            Barriers barrier = child.GetComponent<Barriers>();
            pressurePlate.OnToggle.AddListener(barrier.Move);
        }

        character.OnInventoryShown.AddListener(uiManager.ShowInventory);
        character.OnItemDropped.AddListener(inventoryManager.DropInventory);
    }
    void SpawnInventory(InventoryItem item)
    {
        switch (item)
        {
            case InventoryItem.Weight:
                PlaceInventory(weightPrefab);
                break;
        }
    }

    void PlaceInventory(Inventory inventoryPrefab)
    {
        Inventory inventory = Instantiate(inventoryPrefab);
        inventory.OnItemCollected.AddListener(inventoryManager.PickUpInventory);

        // drop the new inventory item at the player position and a little
        // bit in front of it the player
        inventory.transform.position = character.transform.position + character.transform.forward;
    }
}

