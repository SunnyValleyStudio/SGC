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
    public float jumpSpeed;

    public int angleRotationThreshold;

    public Vector3 moveDirection = Vector3.zero;

    public bool IsGround()
    {
        return characterController.isGrounded;
    }

    protected float desiredRotationAngler = 0;

    int inputVerticalDirection = 0;

    bool isJumping = false;
    bool finishedJumping = true;

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

    public void HandleJump()
    {
        if (characterController.isGrounded)
        {
            isJumping = true;
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
        if (isJumping)
        {
            isJumping = false;
            finishedJumping = false;
            moveDirection.y = jumpSpeed;
            agentAnimations.SetMovementFloat(0);
            agentAnimations.TriggerJumpAnimation();
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void RotateAgent()
    {
        if(desiredRotationAngler > angleRotationThreshold || desiredRotationAngler < -angleRotationThreshold)
        {
            transform.Rotate(Vector3.up * desiredRotationAngler * rotationSpeed * Time.deltaTime);
        }
    }

    public void StpMovementImmediatelly()
    {
        moveDirection = Vector3.zero;
        finishedJumping = false;
    }

    public bool HasFinishedJumping()
    {
        return finishedJumping;
    }

    public void SetFinishedJumping(bool value)
    {
        finishedJumping = value;
    }

    public void SetFinishedJumpingTrue()
    {
        finishedJumping = true;
    }

    public void SetFinishedJumpingFalse()
    {
        finishedJumping = false;
    }
}
