using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    protected CharacterController characterController;
    protected HumanoidAnimations agentAnimations;
    public float movementSpeed;
    public float gravity;
    public float rotationSpeed;

    public int angleRotationThreshold;

    protected Vector3 moveDirection = Vector3.zero;

    protected float desiredRotationAngler = 0;

    int inputVerticalDirection = 0;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        agentAnimations = GetComponent<HumanoidAnimations>();
    }

    public void HandleMovement(Vector2 input)
    {
        if (characterController.isGrounded)
        {
            if (input.y != 0)
            {
                if (input.y > 0)
                {
                    inputVerticalDirection = Mathf.CeilToInt(input.y);
                }
                else
                {
                    inputVerticalDirection = Mathf.FloorToInt(input.y);
                }
                moveDirection = input.y * transform.forward * movementSpeed;
            }
            else
            {
                agentAnimations.SetMovementFloat(0);
                moveDirection = Vector3.zero;
            }
        }

        
    }

    public void HandleMovementDirection(Vector3 input)
    {
        desiredRotationAngler = Vector3.Angle(transform.forward, input);
        var crossProduct = Vector3.Cross(transform.forward, input).y;
        if(crossProduct < 0)
        {
            desiredRotationAngler *= -1;
        }
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            if (moveDirection.magnitude > 0)
            {
                var animationSpeedMultiplier = agentAnimations.SetCorrectAnimation(desiredRotationAngler, angleRotationThreshold, inputVerticalDirection);
                RotateAgent();
                moveDirection *= animationSpeedMultiplier;
            }
        }
        moveDirection.y -= gravity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void RotateAgent()
    {
        if(desiredRotationAngler > angleRotationThreshold || desiredRotationAngler < -angleRotationThreshold)
        {
            transform.Rotate(Vector3.up * desiredRotationAngler * rotationSpeed * Time.deltaTime);
        }
    }
}
