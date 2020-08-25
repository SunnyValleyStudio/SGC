using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        controllerReference.movement.StopMovement();
        controllerReference.agentAnimations.OnFinishedAttacking += TransitionBack;
        controllerReference.agentAnimations.TriggerAttackAnimation();

    }

    private void TransitionBack()
    {
        controllerReference.agentAnimations.OnFinishedAttacking -= TransitionBack;
        controllerReference.TransitionToState(controllerReference.movementState);
    }
}
