using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpController : MonoBehaviour, IPlayerFunctionController {
    
    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpForce = 35;
    [SerializeField] private float coyoteTime = 0.3f;

    [SerializeField] private Transform jumpPoint;
    
    private float coyoteTicker;

    private Rigidbody2D rb;

    private bool isGrounded = false;
    private bool canCoyote = false;

    private PlayerController playerController;
    
    private bool isGrappling;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerController.AddFunction(this);
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        coyoteTicker = coyoteTime;
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        if (isGrounded)
        {
                canCoyote = true;
                coyoteTicker = coyoteTime;
        }

        if (!isGrounded && canCoyote)
        {
            coyoteTicker -= Time.deltaTime;
            if (coyoteTicker <= 0)
            {
                canCoyote = false;
            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrappling) return;
        if (isGrounded || canCoyote)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * 10);
        } 
    }

    private void EndJump(InputAction.CallbackContext context)
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(jumpPoint.position, Vector2.down, 0.3f, ground);
        
        return raycastHit2D.collider != null;
    }

    public void SetKeybinds(KeybindController keybindController)
    {
        keybindController.AddToJumpListener(Jump);
        keybindController.AddToJumpEndListener(EndJump);
    }

    public void ReceiveMessage(string msg)
    {
        switch (msg)
        {
            case PlayerGrappleController.GRAPPLE_STARTED:
                isGrappling = true;
                break;
            case PlayerGrappleController.GRAPPLE_ENDED:
                isGrappling = false;
                break;
        }
    }
}