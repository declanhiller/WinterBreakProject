using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{

    [SerializeField] private Hook hook;
    [SerializeField] private float speed = 30f;

    [SerializeField] private Transform rotation;

    private Vector2 grapplePos;
    private bool isShootingGrapple;
    private bool isGrappleConnected;
    
    // Start is called before the first frame update
    void Start()
    {
        hook.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOnGrapple(Action onGrapple)
    {
        onGrapple.Invoke();
    }

    public void StartGrappleShoot(Vector2 direction, float maxDistance)
    {
        hook.enabled = true;
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
        hook.enabled = false;
    }
}
