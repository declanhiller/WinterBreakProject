using System;
using UnityEngine;
public class Rope : MonoBehaviour
{
    private LineRenderer renderer;

    private float ropeLength;
    private Vector2 anchorPoint;
    private Vector2 endPoint;
    
    private void Start()
    {
        renderer = GetComponent<LineRenderer>();
        //temporary for testing
        renderer.positionCount = 2;
        ropeLength = 7f;
        anchorPoint = new Vector2(0, 0);
        endPoint = new Vector2(-7, 0);
        renderer.SetPosition(0, anchorPoint);
        renderer.SetPosition(1, endPoint);
    }

    private void Update()
    {
        Vector2 direction = (anchorPoint - endPoint).normalized;
        
        renderer.SetPosition(0, anchorPoint);
        renderer.SetPosition(1, endPoint);
    }

    public void Enable(float ropeLength)
    {
        renderer.enabled = true;
        renderer.positionCount = 2;
    }

    public void Disable()
    {
        
    }
}