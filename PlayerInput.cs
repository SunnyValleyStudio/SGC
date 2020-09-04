using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public Vector3 MovementDirectionVector { get; private set; }

    public Action OnJump { get; set; }

    public Action OnToggleInventory { get; set; }

    public Action<int> OnHotbarKey { get; set; }

    public Action OnPrimaryAction { get; set; }

    public Action OnSecondaryAction { get; set; }

    private Camera mainCamera;

    private float previousPrimaryActionInput = 0, prevousSecondaryActionInput = 0;

    public Action OnEscapeKey { get; set; }

    public bool menuState = false;

    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CheckEscapeButton();

        if (menuState == false)
        {
            GetMovementInput();
            GetMovementDirection();
            GetJumpInput();
            GetInventoryInput();
            GetHotbarInput();
            GetPrimaryAction();
            GetSecondaryAction();
        }



    }

    private void CheckEscapeButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            OnEscapeKey?.Invoke();
            
        }
    }

    private void GetSecondaryAction()
    {
        var inputValue = Input.GetAxisRaw("Fire2");
        if (prevousSecondaryActionInput == 0)
        {
            if (inputValue >= 1)
            {
                OnSecondaryAction?.Invoke();
            }
        }
        prevousSecondaryActionInput = inputValue;
    }

    private void GetPrimaryAction()
    {
        var inputValue = Input.GetAxis("Fire1");
        if (previousPrimaryActionInput == 0)
        {
            if (inputValue >= 1)
            {
                OnPrimaryAction?.Invoke();
            }
        }
        previousPrimaryActionInput = inputValue;
    }

    private void GetHotbarInput()
    {
        char hotbar0 = '0';
        for (int i = 0; i < 10; i++)
        {
            KeyCode keyCode = (KeyCode)((int)hotbar0 + i);
            if (Input.GetKeyDown(keyCode))
            {
                OnHotbarKey?.Invoke(i);
                return;
            }
        }
    }

    private void GetInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnToggleInventory?.Invoke();
        }
    }

    private void GetJumpInput()
    {
        if (Input.GetAxisRaw("Jump") > 0)
        {
            OnJump?.Invoke();
        }
    }

    private void GetMovementDirection()
    {
        MovementInputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //Debug.Log(MovementInputVector);
    }

    private void GetMovementInput()
    {
        var cameraForewardDirection = mainCamera.transform.forward;
        //Debug.DrawRay(mainCamera.transform.position, cameraForewardDirection * 10, Color.red);
        MovementDirectionVector = Vector3.Scale(cameraForewardDirection, (Vector3.right + Vector3.forward));
        //Debug.DrawRay(mainCamera.transform.position, MovementDirectionVector * 10, Color.green);
    }
}
