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
    [SerializeField] private Transform spriteTransform;
    
    private float coyoteTicker;

    private Rigidbody2D rb;

    private bool isGrounded = false;
    private bool canCoyote = false;

    private PlayerController playerController;
    
    private bool isGrappling;

    public const string LEFT_GROUND = "LEFT_GROUND";
    public const string LANDED = "LANDED";

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

        bool prevIsGrounded = isGrounded;
        
        isGrounded = IsGrounded();
        
        if (!prevIsGrounded && isGrounded)
        {
            playerController.SendMsg(LANDED);
        }

        if (prevIsGrounded && !isGrounded)
        {
            playerController.SendMsg(LEFT_GROUND);
        }
        
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

        RotateToCorrectLanding();

    }

    private void RotateToCorrectLanding()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(jumpPoint.position, Vector2.down, 0.2f, ground);
        if (raycastHit2D.collider != null)
        {
            Vector2 normal = raycastHit2D.normal;
            spriteTransform.rotation = Quaternion.FromToRotation(spriteTransform.up, normal) * spriteTransform.rotation;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrappling) return;
        if (isGrounded || canCoyote)
        {
            playerController.SendMsg(LEFT_GROUND);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * 10);
            StartCoroutine(RotatePlayerToVerticalPosition());
        } 
    }

    IEnumerator RotatePlayerToVerticalPosition()
    {
        float eulerAnglesZ = spriteTransform.eulerAngles.z;
        int direction = eulerAnglesZ < 180 ? -1 : 1;
        while (eulerAnglesZ != 0)
        {
            float zAngleChange = 150 * Time.deltaTime * direction;
            bool setToZero = false;
            if (direction == 1)
            {
                if (zAngleChange + eulerAnglesZ > 360)
                {
                    setToZero = true;
                }
            } else
            {
                if (eulerAnglesZ + zAngleChange < 0)
                {
                    setToZero = true;
                }
            }

            spriteTransform.Rotate(0, 0, zAngleChange);
            if (setToZero)
            {
                var eulerAngles = spriteTransform.eulerAngles;
                eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);
                spriteTransform.eulerAngles = eulerAngles;
            }
            eulerAnglesZ = spriteTransform.eulerAngles.z;
            yield return new WaitForEndOfFrame();
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
        RaycastHit2D raycastHit2D = Physics2D.Raycast(jumpPoint.position, Vector2.down, 0.05f, ground);

        bool grounded = raycastHit2D.collider != null;

        // if (grounded && canCheckForGroundTicks)
        // {
        //     if (numberOfTicksOnGround == 0)
        //     {
        //         rb.gravityScale = 0;
        //         playerController.SendMsg(LANDED);
        //     }
        //     numberOfTicksOnGround++;
        // }
        
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
        return 0;
    }
}