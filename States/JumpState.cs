using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        controllerReference.movement.HandleJump();
    }

    public override void Update()
    {
        controllerReference.TransitionToState(controllerReference.movementState);
    }
}
