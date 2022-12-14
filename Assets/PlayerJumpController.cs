using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpController : MonoBehaviour {
    
    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpForce = 35;


    private KeybindController keybindController;
    private Rigidbody2D rb;
    private void Start() {
        keybindController = GetComponent<KeybindController>();
        keybindController.AddToJumpListener(Jump);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Jump(InputAction.CallbackContext context) {
        if (IsGrounded()) {
            rb.AddForce(Vector2.up * jumpForce * 10);
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 1f, ground);
        return raycastHit2D.collider != null;
    }
}