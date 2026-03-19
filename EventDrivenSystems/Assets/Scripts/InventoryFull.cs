using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFull : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void ShowInventoryFull()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("SetShake");
    }
    public void HideInventoryFull()
    {
        gameObject.SetActive(false);
    }
}
