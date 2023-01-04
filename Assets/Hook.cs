using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;
    private SpriteRenderer renderer;

    [SerializeField]
    private Rope rope;

    [SerializeField] private Transform firepoint;
    [SerializeField] private Transform player;
    
    [SerializeField] private LayerMask grappleMask;

    private float maxDistance;
    private Vector2 maxTarget;

    private bool didHook;
    private bool missed;
    private bool move;

    private Vector2 direction;
    private float speed;
    private float defaultGravityScale;

    [SerializeField] private float launchSpeed = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
    }

    private void Update()
    {

        float proposedTravelDistance = speed * Time.deltaTime;
        float travelDistance = proposedTravelDistance;

        bool setMove = true;
        
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, proposedTravelDistance, grappleMask);
        if (hit2D.collider != null)
        {
            travelDistance = hit2D.distance;
            setMove = false;
            // joint.enabled = true;
            didHook = true;
        }
        
        if(maxDistance * maxDistance < MathUtils.SquaredDistance(firepoint.transform.position, transform.position + (Vector3) (proposedTravelDistance * direction)))
        {
            travelDistance = ((firepoint.transform.position + (Vector3) (maxDistance * direction)) - transform.position).magnitude;
            setMove = false;
        }
        
        


        if (move)
        {
            transform.Translate(travelDistance * direction);
        }
        
        rope.DrawRope(firepoint.transform.position, transform.position);

        move = setMove;
    }

    public void ShootInDirection(Vector2 direction, float speed, float maxDistance)
    {
        rope.enabled = true;
        move = true;
        transform.position = firepoint.position;
        this.maxDistance = maxDistance;
        this.maxTarget = (Vector2) transform.position + (direction * this.maxDistance);
        this.direction = direction.normalized;
        this.speed = speed;
    }


    private void OnEnable()
    {
        collider.enabled = true;
        renderer.enabled = true;
        rope.enabled = true;
    }

    private void OnDisable()
    {
        collider.enabled = false;
        renderer.enabled = false;
        rope.enabled = false;
        move = false;
        missed = false;
        didHook = false;
        rb.velocity = Vector2.zero;
        transform.position = firepoint.position;
    }

    public bool DidHookConnect()
    {
        return didHook;
    }
}
