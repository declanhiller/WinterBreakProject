using System;
using UnityEditor;
using UnityEngine;
public class GrappleAimer : MonoBehaviour
{

    private SpriteRenderer renderer;
    
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        Disable();
    }

    public void UpdateCrosshair(Vector2 mousePos)
    {
        transform.right = mousePos - (Vector2) this.transform.position;
    }

    public void Enable()
    {
        renderer.enabled = true;
    }

    public void Disable()
    {
        renderer.enabled = false;
    }
}