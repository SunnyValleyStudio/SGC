﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour, ISavable
{
    public AgentMovement movement;
    public PlayerInput input;

    public HumanoidAnimations agentAnimations;

    public InventorySystem inventorySystem;

    public CraftingSystem craftingSystem;

    public DetectionSystem detectionSystem;

    public GameManager gameManager;

    public Transform itemSlot;

    public AudioSource audioSource;

    public BuildingPlacementStorage buildingPlacementStroage;

    public Vector3? spawnPosition = null;

    public PlayerStatsManager playerStatsManager;

    BaseState currentState;
    public readonly BaseState movementState = new MovementState();
    public readonly BaseState jumpState = new JumpState();
    public readonly BaseState fallingState = new FallingState();
    public readonly BaseState inventoryState = new InventoryState();
    public readonly BaseState interactState = new InteractState();
    public readonly BaseState attackState = new AttackState();
    public readonly BaseState placementState = new PlacementState();

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
        inventorySystem.OnStructureUse += StructureUseCallback;
        craftingSystem.onCheckResourceAvailability += inventorySystem.CheckResourceAvailability;
        craftingSystem.onCheckInventoryFull += inventorySystem.CheckInventoryFull;
        craftingSystem.onCraftItemRequest += inventorySystem.CraftAnItem;
        inventorySystem.onInventoryStateChanged += craftingSystem.RecheckIngredients;
    }

    private void StructureUseCallback()
    {
        if (inventorySystem.InventoryVisible)
        {
            inventorySystem.ToggleInventory();
            craftingSystem.ToggleCraftingUI();
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
        }
        TransitionToState(placementState);
    }

    private void AssignInputListeners()
    {
        input.OnJump += HandleJump;
        input.OnHotbarKey += HandleHotbarInput;
        input.OnToggleInventory += HandleInventoryInput;
        input.OnPrimaryAction += HandlePrimaryInput;
        input.OnSecondaryAction += HandleSecondaryInput;
        input.OnEscapeKey += HendleEscapeKey;
    }

    private void HendleEscapeKey()
    {
        currentState.HandleEscapeInput();
        
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
        if (Time.timeScale == 0)
            return;
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

    public void PlayWeaponSwooshSOund()
    {
        audioSource.PlayOneShot(AudioLibrary.instance.weaponWoosh);
    }

    internal void SaveSpawnPoint()
    {
        spawnPosition = transform.position;
    }

    private void RespawnPlayer()
    {
        if (spawnPosition != null)
        {
            movement.TeleportPlayerTo(spawnPosition.Value + Vector3.up);
        }
    }

    public string GetJsonDataToSave()
    {
        var data = new PositionStruct
        {
            x = spawnPosition.Value.x,
            y = spawnPosition.Value.y,
            z = spawnPosition.Value.z
        };

        return JsonConvert.SerializeObject(data);
    }

    public void LoadJsonData(string jsonData)
    {
        var data = JsonConvert.DeserializeObject<PositionStruct>(jsonData);
        spawnPosition = new Vector3(data.x, data.y, data.z);
        RespawnPlayer();
    }
}

[Serializable]
public struct PositionStruct
{
    public float x, y, z;
}
