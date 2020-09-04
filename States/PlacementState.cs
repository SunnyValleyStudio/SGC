using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : MovementState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
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
