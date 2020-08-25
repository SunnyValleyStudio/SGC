using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : BaseState
{

    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        Debug.Log("Open inventory window");
        controllerReference.inventorySystem.ToggleInventory();
        controllerReference.craftingSystem.ToggleCraftingUI();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void HandleInventoryInput()
    {
        base.HandleInventoryInput();
        Debug.Log("Close Inventory");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controllerReference.inventorySystem.ToggleInventory();
        controllerReference.craftingSystem.ToggleCraftingUI();
        controllerReference.TransitionToState(controllerReference.movementState);
        
    }

    public override void HandleEscapeInput()
    {
        //base.HandleEscapeInput();
        //controllerReference.inventorySystem.ToggleInventory();
        //controllerReference.craftingSystem.ToggleCraftingUI(true);
        HandleInventoryInput();
    }

}
