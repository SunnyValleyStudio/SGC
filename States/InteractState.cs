using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        Debug.Log("Entering interact state");
    }

    public override void Update()
    {
        base.Update();
        controllerReference.TransitionToState(controllerReference.movementState);
    }
}
