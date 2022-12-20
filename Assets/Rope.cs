using System;
using UnityEngine;
public class Rope : MonoBehaviour
{
    private LineRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<LineRenderer>();
    }


    private void OnEnable()
    {
        renderer.positionCount = 2;
        renderer.enabled = true;
    }

    private void OnDisable()
    {
        renderer.enabled = false;
    }
    
    
    public void DrawRope(Vector2 playerPos, Vector2 grapplePos)
    {
        renderer.SetPosition(0, playerPos);
        renderer.SetPosition(1, grapplePos);
    }
}