using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    CharacterController controller;
    Vector3 velocity;
    Animator animator;
    Collider shovelCollider;

    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;

    public InputActionReference moveInput;
    public InputActionReference attackInput;
    public InputActionReference weaponSwitchInput;
    public InputActionReference dropInventoryInput;
    public InputActionReference showInventoryInput;

    [HideInInspector]
    public UnityEvent OnItemDropped;

    [HideInInspector]
    public UnityEvent<bool> OnInventoryShown;

    public Shovel shovel;

    public GameObject armRight;

    bool shortRangeAttack = true;
    bool showInventory = false;

    void Awake()
    {
        if (OnItemDropped == null) OnItemDropped = new UnityEvent();

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        shovelCollider = shovel.GetComponent<Collider>();
        shovelCollider.enabled = false;

        shortRangeAttack = true;

        // this is a C# delegate. we use += to add a listener to a delegate
        dropInventoryInput.action.performed += DropInventoryPerformed;

        showInventoryInput.action.performed += ShowInventoryPerformed;
        showInventoryInput.action.canceled += ShowInventoryCanceled;
    }

    private void ShowInventoryCanceled(InputAction.CallbackContext obj)
    {
        showInventory = false;
        OnInventoryShown.Invoke(showInventory);
    }

    private void ShowInventoryPerformed(InputAction.CallbackContext obj)
    {
        showInventory = true;
        OnInventoryShown.Invoke(showInventory);
    }

    private void DropInventoryPerformed(InputAction.CallbackContext obj)
    {
        // should you be able to drop inventory with the inventory panel open?
        // yes
        OnItemDropped.Invoke();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool inputEnabled = !showInventory;

        PlayerMotion(inputEnabled);

        bool attack = attackInput.action.WasPressedThisFrame();
        if (attack && inputEnabled)
        {
            if (shortRangeAttack)
            {
                // short range attack always works (no cooldown)
                animator.SetTrigger("StartAttack");
            }
        }
    }

    void PlayerMotion(bool inputEnabled)
    {
        if (!inputEnabled) return;

        // the following is pretty standard character controller code

        // snap the player to the ground if already grounded
        // when jumping, gravity takes over
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 moveDirection = moveInput.action.ReadValue<Vector2>();

        Vector3 move = Vector3.right * moveDirection.x + Vector3.forward * moveDirection.y;
        Vector3 moveVelocity = move * moveSpeed;

        // allow gravity to impact y velocity
        velocity.y += gravity * Time.deltaTime;

        moveVelocity.y = velocity.y;

        // finally, Move the character
        controller.Move(moveVelocity * Time.deltaTime);


        // rotate the character using Quaternion LookRotation()
        // slerp = Spherical Linear Interpolation. smoothly interpolates between Quaternion rotations
        Vector3 horizontalVelocity = new Vector3(moveVelocity.x, 0f, moveVelocity.z);
        if (horizontalVelocity.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                15f * Time.deltaTime
            );
        }

        // set the speed for the animator to change idle/walk states
        animator.SetFloat("Speed", horizontalVelocity.magnitude);
    }

    public void EnableHitbox(int value)
    {
        // only the shovel should be enabled from the swinging animation
        if (shortRangeAttack)
        {
            shovel.EnableHitbox(value);
        }
    }
}
