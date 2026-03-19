using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public TextMeshProUGUI weightsText;

    public GameObject inventoryPanel;

    public TextMeshProUGUI activeInventoryText;
    public void Awake()
    {
        inventoryPanel.SetActive(false);
    }
    public void UpdateInventoryUI()
    {
        // get the inventory quantity directly from the inventory manager
        int weightsInventory = inventoryManager.inventory[InventoryItem.Weight];
        weightsText.text = $"Weights: {weightsInventory}";
    }

    public void ShowInventory(bool show)
    {
        Debug.Log("show Inventory");
        inventoryPanel.SetActive(show);
    }

    public void SetWeightActive()
    {
        SetInventoryActive(InventoryItem.Weight);
    }

    void SetInventoryActive(InventoryItem item)
    {
        inventoryManager.activeItem = item;
        activeInventoryText.text = $"Active Inventory: {item}";
    }

}
