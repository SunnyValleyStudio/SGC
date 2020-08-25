using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public AgentMovement movement;
    public PlayerInput input;

    private void OnEnable()
    {
        movement = GetComponent<AgentMovement>();
        input = GetComponent<PlayerInput>();
        input.OnJump += movement.HandleJump;
    }

    private void Update()
    {
        movement.HandleMovement(input.MovementInputVector);
        movement.HandleMovementDirection(input.MovementDirectionVector);
    }


    private void OnDisable()
    {
        input.OnJump -= movement.HandleJump;
    }
}
