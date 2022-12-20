using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{

    [SerializeField] private Rope rope;
    [SerializeField] private Hook hook;
    [SerializeField] private float speed = 15f;

    [SerializeField] private Transform rotation;


    private Vector2 grapplePos;
    private bool isShootingGrapple;
    private bool isGrappleConnected;
    
    // Start is called before the first frame update
    void Start()
    {
        rope.enabled = false;
        hook.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        // if (isShootingGrapple)
        // {
        //     Vector2 hookPosition = hook.GetPosition();
        //     rope.DrawRope(transform.position, hookPosition);
        //     isGrappleConnected = hook.DidHook();
        //     if (isGrappleConnected)
        //     {
        //         isShootingGrapple = false;
        //     } else if (hook.Missed())
        //     {
        //         //do some falling animation where character rewinds rope back or something.
        //         // hook.enabled = false;
        //         // rope.enabled = false;
        //     }
        // } else if (isGrappleConnected)
        // {
        //     
        // }
    }

    public void SetOnGrapple(Action onGrapple)
    {
        onGrapple.Invoke();
    }

    public void StartGrappleShoot(Vector2 direction, float maxDistance)
    {
        hook.enabled = true;
        rope.enabled = true;
        isShootingGrapple = true;
        hook.MoveInDirection(direction, speed, maxDistance);
    }

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        isShootingGrapple = false;
        isGrappleConnected = false;
        rope.enabled = false;
        hook.enabled = false;
    }
}
