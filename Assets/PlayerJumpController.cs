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

    public const string JUMP_STARTED = "JUMP_STARTED";
    public const string LANDED = "LANDED";

    public int numberOfTicksOnGround = 0;
    private bool canCheckForGroundTicks = true;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerController.AddFunction(this);
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        coyoteTicker = coyoteTime;
    }

    public void Tick()
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
            numberOfTicksOnGround = 0;
            canCheckForGroundTicks = false;
            playerController.SendMsg(JUMP_STARTED);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * 10);
            Invoke(nameof(EnableCheckForGroundTicks), 0.1f);
        } 
    }

    void EnableCheckForGroundTicks()
    {
        canCheckForGroundTicks = true;
    }
    

    private void EndJump(InputAction.CallbackContext context)
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(jumpPoint.position, Vector2.down, 0.05f, ground);

        bool grounded = raycastHit2D.collider != null;

        if (grounded && canCheckForGroundTicks)
        {
            if (numberOfTicksOnGround == 0)
            {
                playerController.SendMsg(LANDED);
            }
            numberOfTicksOnGround++;
        }
        
        return grounded;
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

    public int GetPriority()
    {
        throw new NotImplementedException();
    }
}