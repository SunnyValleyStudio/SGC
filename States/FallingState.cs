using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : JumpState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        controllerReference.agentAnimations.TriggerFallAnimation();
        controllerReference.movement.SetFinishedJumpingFalse();
    }

    public override void Update()
    {
        base.Update();
    }
}
