using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace L
{
    public class InputHandler : MonoBehaviour
 {
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool a_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool jump_Input;
    public bool inventory_Input;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;


    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UIManager uIManager;

    Vector2 movementInput;
    Vector2 cameraInput;


    private void Awake()
    {
        playerAttacker = GetComponentInChildren<PlayerAttacker>();
        playerInventory = GetComponentInChildren<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        uIManager = FindObjectOfType<UIManager>();
        
    }




    public void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollingInput(delta);
        HandleAttackInput(delta);
        HandleQuickslotsInput();
        HandleInteractableInput();
        HandleJumpInput();
        HandleInventoryInput();
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;


    }

    private void HandleRollingInput(float delta)
    {
        b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        if(b_Input)
        {
            rollInputTimer += delta;
            sprintFlag = true;

        }
        else
        {
            if(rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }
            
            rollInputTimer = 0;
        }
    }

    private void HandleAttackInput(float delta)
    {
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;
        if(rb_Input)
        {
            if(playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if(playerManager.isInteracting)
                return;
                if(playerManager.canDoCombo)
                return;
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);

            }

        }

        if(rt_Input)
        {
            if(playerManager.canDoCombo)
            {
             comboFlag = true;
             playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
             comboFlag = false;
            }
            else
            {
                if(playerManager.isInteracting)
                return;
                if(playerManager.canDoCombo)
                return;
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);

            }
            
        }
    }

    private void HandleQuickslotsInput()
    {
        inputActions.Quickslots.DPadRight.performed += i => d_Pad_Right = true;
        inputActions.Quickslots.DPadLeft.performed += i => d_Pad_Left = true;
        if(d_Pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if(d_Pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
        }
    }

    void HandleInteractableInput()
    {
        inputActions.PlayerActions.A.performed += i => a_Input = true;

    }

    void HandleJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;

    }
    void HandleInventoryInput()
    {
        inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;

        if(inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if(inventoryFlag)
            {
                uIManager.OpenSelectWindow();
                uIManager.UpdateUI();
                uIManager.hudWindow.SetActive(false);
            }
            else
            {
                uIManager.CloseSelectWindow();
                uIManager.CloseAllInventoryWindows();
                uIManager.hudWindow.SetActive(true);
            }
        }
    }
 }
}