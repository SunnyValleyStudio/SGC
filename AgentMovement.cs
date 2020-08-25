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

    protected float desiredRotationAngle = 0;

    int inputVerticalDirection = 0;

    bool isJumping = false;
    bool finishedJumping = true;

    private bool temporareyMovementTriggered = false;
    private Quaternion endRotationY;
    private float temporaryDesiredRotationAngle;



    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        agentAnimations = GetComponent<HumanoidAnimations>();
    }

    public bool IsGround()
    {
        return characterController.isGrounded;
    }

    public void HandleMovement(Vector2 input)
    {
        if (characterController.isGrounded)
        {
            if (input.y != 0)
            {
                temporareyMovementTriggered = false;
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
                if(input.x != 0)
                {
                    if(temporareyMovementTriggered == false)
                    {
                        temporareyMovementTriggered = true;

                        int directionParameter = input.x > 0 ? 1 : -1;
                        if(directionParameter > 0)
                        {
                            temporaryDesiredRotationAngle = 90;
                        }
                        else
                        {
                            temporaryDesiredRotationAngle = -90;
                        }
                        endRotationY = Quaternion.Euler(transform.localEulerAngles) * Quaternion.Euler(Vector3.up * temporaryDesiredRotationAngle);
                    }
                    inputVerticalDirection = 1;
                    moveDirection = transform.forward * movementSpeed;


                }
                else
                {
                    temporareyMovementTriggered = false;
                    agentAnimations.SetMovementFloat(0);
                    moveDirection = Vector3.zero;
                }
                
            }
        }

        
    }

    public void HandleMovementDirection(Vector3 input)
    {
        if (temporareyMovementTriggered)
        {
            return;
        }
        desiredRotationAngle = Vector3.Angle(transform.forward, input);
        var crossProduct = Vector3.Cross(transform.forward, input).y;
        if(crossProduct < 0)
        {
            desiredRotationAngle *= -1;
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
            if (moveDirection.magnitude > 0 && finishedJumping)
            {
                var animationSpeedMultiplier = agentAnimations.SetCorrectAnimation(desiredRotationAngle, angleRotationThreshold, inputVerticalDirection);
                if (temporareyMovementTriggered == false)
                {
                    RotateAgent();
                }
                else
                {
                    RotateTemp();
                }
                
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

    private void RotateTemp()
    {
        desiredRotationAngle = Quaternion.Angle(transform.rotation, endRotationY);
        if(desiredRotationAngle > angleRotationThreshold || desiredRotationAngle < -angleRotationThreshold)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, endRotationY, Time.deltaTime * rotationSpeed * 100);
        }
    }

    private void RotateAgent()
    {
        if(desiredRotationAngle > angleRotationThreshold || desiredRotationAngle < -angleRotationThreshold)
        {
            transform.Rotate(Vector3.up * desiredRotationAngle * rotationSpeed * Time.deltaTime);
        }
    }

    public void StopMovementImmediatelly()
    {
        moveDirection = Vector3.zero;
        finishedJumping = false;
    }

    public void StopMovement()
    {
        moveDirection = Vector3.zero;
        desiredRotationAngle = 0;
        agentAnimations.SetMovementFloat(0);
        inputVerticalDirection = 0;
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
