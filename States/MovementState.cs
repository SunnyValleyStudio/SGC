using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : BaseState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
    }

    public override void HandleMovement(Vector2 input)
    {
        base.HandleMovement(input);
    }

    public override void HandleCameraDirection(Vector3 input)
    {
        base.HandleCameraDirection(input);
    }

    public override void HandleJumpInput()
    {
        base.HandleJumpInput();
    }

    public override void Update()
    {
        base.Update();
    }
}
