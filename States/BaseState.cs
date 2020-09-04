using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected AgentController controllerReference;

    public virtual void EnterState(AgentController controller)
    {
        this.controllerReference = controller;
    }

    public virtual void HandleMovement(Vector2 input) { }

    public virtual void HandleCameraDirection(Vector3 input) { }

    public virtual void HandleJumpInput() { }

    public virtual void HandleInventoryInput() { }

    public virtual void HandleHotbarInput(int hotbarKey) 
    {
        Debug.Log(hotbarKey);
    }

    public virtual void Update() {}

    public virtual void HandlePrimaryAction()
    {
    }

    public virtual void HandleSecondaryAction()
    {
    }

    public virtual void HandleEscapeInput()
    {
        controllerReference.gameManager.ToggleGameMenu();
        if (controllerReference.input.menuState == false)
        {
            controllerReference.input.menuState = true;
        }
        else
        {
            controllerReference.input.menuState = false;
        }
    }
}
