using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpController : MonoBehaviour {
    
    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpForce = 35;
    [SerializeField] private float coyoteTime = 0.3f;
    private float coyoteTicker;

    private KeybindController keybindController;
    private Rigidbody2D rb;

    private bool isGrounded = false;
    private bool canCoyote = false;


    private void Start() {
        keybindController = GetComponent<KeybindController>();
        keybindController.AddToJumpListener(Jump);
        keybindController.AddToJumpEndListener(EndJump);
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


    private void Jump(InputAction.CallbackContext context) {
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
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, ground);
        
        return raycastHit2D.collider != null;
    }
}