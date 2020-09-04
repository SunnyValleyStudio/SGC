using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : MovementState
{
    PlacementHelper placementHelper;
    public override void EnterState(AgentController controller)
    {
        Debug.Log("Entering placement state");
        base.EnterState(controller);
        CreateStructureToPlace();
    }

    private void CreateStructureToPlace()
    {
        placementHelper = ItemSpawnManager.instance.CreateStructure(controllerReference.inventorySystem.selectedStructureData);
        placementHelper.PrepareForMovement();
        Debug.Log("Creating a structure to palce");
    }

    public override void HandleEscapeInput()
    {
        Debug.Log("Exiting placementState");
        DestroyPlacedObject();
        controllerReference.TransitionToState(controllerReference.movementState);
    }

    public override void HandleJumpInput()
    {
    }

    public override void HandleInventoryInput()
    {
    }

    public override void HandleSecondaryAction()
    {

    }

    public override void HandlePrimaryAction()
    {
    }

    public override void HandleHotbarInput(int hotbarKey)
    {
    }

    public override void Update()
    {
        HandleMovement(controllerReference.input.MovementInputVector);
        HandleCameraDirection(controllerReference.input.MovementDirectionVector);
        HandleFallingDown();
    }

    protected new void HandleFallingDown()
    {
        if (controllerReference.movement.IsGround() == false)
        {
            if (fallingDelay > 0)
            {
                fallingDelay -= Time.deltaTime;
                return;
            }
            DestroyPlacedObject();
            controllerReference.TransitionToState(controllerReference.fallingState);
        }
        else
        {
            fallingDelay = defaultFallingDelay;
        }
    }

    private void DestroyPlacedObject()
    {
        Debug.Log("Destroying placed object");
    }
}
