using System;
using UnityEditor;
using UnityEngine;
public class GrappleAimer : MonoBehaviour
{
    
    private SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
    }

    public void UpdateCrosshair(Vector2 mousePos)
    {
        transform.right = mousePos - (Vector2) this.transform.position;
    }

    public Vector2 GetDirection()
    {
        return transform.right.normalized;
    }

    private void OnEnable()
    {
        renderer.enabled = true;
    }

    private void OnDisable()
    {
        renderer.enabled = false;
    }
}