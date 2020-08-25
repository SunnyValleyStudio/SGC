using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    protected CharacterController characterController;
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
                moveDirection = Vector3.zero;
            }
        }

        
    }


    private void Update()
    {
        moveDirection.y -= gravity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

}
