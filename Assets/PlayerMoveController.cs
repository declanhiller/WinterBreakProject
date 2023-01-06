using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour, IPlayerFunctionController {

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 100f;

    [SerializeField] private Transform playerFeet;
    [SerializeField] private LayerMask groundMask;
    
    private KeybindController keybindController;
    private PlayerController playerController;
    private Rigidbody2D rb;

    private bool isFullStopping;

    private bool isFlyingThroughAir;

    //1 for right, -1 for left, 0 for neither
    private int direction = 0;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerController.AddFunction(this);
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Tick()
    {
        float inputtedValue = keybindController.ReadRunValue();

        Vector2 velocity = Vector2.zero;
        

        velocity = GroundedMovement(inputtedValue);

        rb.velocity = velocity;

        if (!isFlyingThroughAir)
        {
            RotateToNormalsOfGround(velocity.x);
        }
        

    }
    
    
    

    private void RotateToNormalsOfGround(float velocity)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(playerFeet.position, Vector2.down, 1f, groundMask);
        if (raycastHit2D.collider != null)
        {
            Vector2 normal = raycastHit2D.normal;
            Debug.Log(normal);
            transform.rotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
        }
    }

    private Vector2 AirMovement(float inputtedValue)
    {
        return Vector2.zero;
    }

    public Vector2 GroundedMovement(float inputtedValue)
    {
        float currentHorizontalSpeed = rb.velocity.x;

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
        
        return new Vector2(currentHorizontalSpeed, rb.velocity.y);
    }

    public void SetKeybinds(KeybindController keybindController)
    {
        this.keybindController = keybindController;
    }

    public void ReceiveMessage(string msg)
    {
        switch (msg)
        {
            case PlayerJumpController.JUMP_STARTED:
                isFlyingThroughAir = true;
                break;
            case PlayerJumpController.LANDED:
                isFlyingThroughAir = false;
                break;
        }
    }

    public int GetPriority()
    {
        throw new NotImplementedException();
    }
}
