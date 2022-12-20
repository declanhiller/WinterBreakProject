using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;
    private SpriteRenderer renderer;

    [SerializeField] private Transform firepoint;
    
    [SerializeField] private LayerMask grappleMask;

    private float maxDistance;
    private Vector2 maxTarget;

    private bool didHook;
    private bool missed;
    private bool move;

    private Vector2 direction;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (move)
        {
            transform.Translate(speed * direction * Time.deltaTime);
        }
        
        
        
        if (maxDistance * maxDistance <= MathUtils.SquaredDistance(firepoint.transform.position, transform.position))
        {
            missed = true;
            move = false;
        }
    }

    public void MoveInDirection(Vector2 direction, float speed, float maxDistance)
    {
        move = true;
        transform.position = firepoint.position;
        this.maxDistance = maxDistance;
        this.maxTarget = (Vector2) transform.position + (direction * this.maxDistance);
        this.direction = direction.normalized;
        this.speed = speed;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        didHook = true;
        move = false;
    }

    public bool DidHook()
    {
        return didHook;
    }

    public bool Missed()
    {
        return missed;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }


    private void OnEnable()
    {
        collider.enabled = true;
        renderer.enabled = true;
    }

    private void OnDisable()
    {
        collider.enabled = false;
        renderer.enabled = false;
        move = false;
        missed = false;
        didHook = false;
        rb.velocity = Vector2.zero;
        transform.position = firepoint.position;

    }
}
