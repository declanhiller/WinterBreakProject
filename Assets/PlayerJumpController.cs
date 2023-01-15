using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpController : MonoBehaviour, IPlayerFunctionController {
    
    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpForce = 35;
    [SerializeField] private float coyoteTime = 0.3f;
    [SerializeField] private float xDist = 0.3f;
    [SerializeField] private float yDist = 0.01f;

    [SerializeField] private Transform jumpPoint;
    
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
            transform.rotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
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

    //flaky on rotating to actual vertical rotation and rotates infinitely until touchdown sometimes
    IEnumerator RotatePlayerToVerticalPosition()
    {
        float eulerAnglesZ = transform.eulerAngles.z;
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

            transform.Rotate(0, 0, zAngleChange);
            if (setToZero)
            {
                var eulerAngles = transform.eulerAngles;
                eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);
                transform.eulerAngles = eulerAngles;
            }
            eulerAnglesZ = transform.eulerAngles.z;
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


        // Collider2D overlapBox = Physics2D.OverlapBox(jumpPoint.position, new Vector2(xDist, yDist), transform.eulerAngles.z, ground);

        // Vector3 point1 = jumpPoint.position + new Vector3(xDist, yDist);
        // Vector3 point2 = jumpPoint.position + new Vector3(xDist, -yDist);
        // Vector3 point3 = jumpPoint.position + new Vector3(-xDist, -yDist);
        // Vector3 point4 = jumpPoint.position + new Vector3(-xDist, yDist);
        //
        // Debug.DrawLine(point1, point2, Color.black);
        // Debug.DrawLine(point2, point3, Color.black);
        // Debug.DrawLine(point3, point4, Color.black);
        // Debug.DrawLine(point4, point1, Color.black);

        bool grounded = raycastHit2D.collider != null;
        // bool grounded = overlapBox != null;

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