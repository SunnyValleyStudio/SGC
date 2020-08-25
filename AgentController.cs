using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public AgentMovement movement;
    public PlayerInput input;
    public HumanoidAnimations agentAnimations;

    public InventorySystem inventorySystem;

    public CraftingSystem craftingSystem;

    public DetectionSystem detectionSystem;

    public GameManager gameManager;

    public Transform itemSlot;

    BaseState currentState;
    public readonly BaseState movementState = new MovementState();
    public readonly BaseState jumpState = new JumpState();
    public readonly BaseState fallingState = new FallingState();
    public readonly BaseState inventoryState = new InventoryState();
    public readonly BaseState interactState = new InteractState();
    public readonly BaseState attackState = new AttackState();

    private void OnEnable()
    {
        movement = GetComponent<AgentMovement>();
        input = GetComponent<PlayerInput>();
        agentAnimations = GetComponent<HumanoidAnimations>();
        currentState = movementState;
        currentState.EnterState(this);
        AssignInputListeners();
        detectionSystem = GetComponent<DetectionSystem>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        craftingSystem.onCheckResourceAvailability += inventorySystem.CheckResourceAvailability;
        craftingSystem.onCheckInventoryFull += inventorySystem.CheckInventoryFull;
        craftingSystem.onCraftItemRequest += inventorySystem.CraftAnItem;
        inventorySystem.onInventoryStateChanged += craftingSystem.RecheckIngredients;
    }

    private void AssignInputListeners()
    {
        input.OnJump += HandleJump;
        input.OnHotbarKey += HandleHotbarInput;
        input.OnToggleInventory += HandleInventoryInput;
        input.OnPrimaryAction += HandlePrimaryInput;
        input.OnSecondaryAction += HandleSecondaryInput;
        input.OnMenuToggledKey += HendleMenu;
    }

    private void HendleMenu()
    {
        currentState.HandleMenuInput();
        gameManager.ToggleGameMenu();
    }

    private void HandleSecondaryInput()
    {
        currentState.HandleSecondaryAction();
    }

    private void HandlePrimaryInput()
    {
        currentState.HandlePrimaryAction();
    }

    private void HandleJump()
    {
        currentState.HandleJumpInput();
    }

    private void HandleInventoryInput()
    {
        currentState.HandleInventoryInput();
    }

    private void HandleHotbarInput(int hotbarkey)
    {
        currentState.HandleHotbarInput(hotbarkey);
    }

    private void Update()
    {
        
        currentState.Update();
    }


    private void OnDisable()
    {
        input.OnJump -= currentState.HandleJumpInput;
    }

    public void TransitionToState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + input.MovementDirectionVector, detectionSystem.detectionRadius);
        }
    }


}
