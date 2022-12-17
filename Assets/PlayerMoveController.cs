using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 100f;

    private KeybindController keybindController;
    private Rigidbody2D rb;

    private bool isFullStopping;

    //1 for right, -1 for left, 0 for neither
    private int direction = 0;
    
    void Start() {
        keybindController = GetComponent<KeybindController>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        float currentHorizontalSpeed = rb.velocity.x;
        float inputtedValue = keybindController.ReadRunValue();
        currentHorizontalSpeed += inputtedValue * acceleration * Time.deltaTime * 10;
        currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -moveSpeed, moveSpeed);
        if (inputtedValue == 0 && currentHorizontalSpeed != 0)
        {
            isFullStopping = true;
        }
        else
        {
            isFullStopping = false;
        }

        if (inputtedValue > 0)
        {
            direction = 1;
        } else if (inputtedValue < 0)
        {
            direction = -1;
        }


        if (isFullStopping)
        {
            currentHorizontalSpeed -= direction * acceleration * Time.deltaTime * 10;
            if (currentHorizontalSpeed <= 0 && direction > 0 || currentHorizontalSpeed >= 0 && direction < 0)
            {
                currentHorizontalSpeed = 0;
                isFullStopping = false;
                direction = 0;
            }
        } else if (inputtedValue > 0 && direction < 0 || inputtedValue < 0 && direction > 0) //let player change directions on a dime instead of slowly adding force in the other direction
        {
            currentHorizontalSpeed = 0;
            
            currentHorizontalSpeed += inputtedValue * acceleration * Time.deltaTime * 10;
            direction = inputtedValue > 0 ? 1 : -1;
        }
        
        rb.velocity = new Vector2(currentHorizontalSpeed, rb.velocity.y);
    }
}
