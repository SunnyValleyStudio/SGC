using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        Debug.Log("Entering interact state");

        var usableStructure = controllerReference.detectionSystem.IUsableCollider;
        if(usableStructure != null)
        {
            usableStructure.GetComponent<IUsable>().Use();
            return;
        }

        var resultCollider = controllerReference.detectionSystem.CurrentCollider;
        if (resultCollider != null)
        {
            var ipickable = resultCollider.GetComponent<IPickable>();
            var remainder = controllerReference.inventorySystem.AddToStorage(ipickable.PickUp());
            ipickable.SetCount(remainder);
            if (remainder > 0)
            {
                Debug.Log("Can't pick it up ");
            }
        }
    }

    public override void Update()
    {
        base.Update();
        controllerReference.TransitionToState(controllerReference.movementState);
    }
}
