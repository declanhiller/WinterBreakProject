using System;
using System.Collections.Generic;
using UnityEngine;
public class Rope : MonoBehaviour
{
    private LineRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void DrawRope(Vector2 position1, Vector2 position2)
    {
        renderer.SetPosition(0, position1);
        renderer.SetPosition(1, position2);
    }

    private void OnEnable()
    {
        Debug.Log("weeeeeeeee");
        renderer.enabled = true;
    }

    private void OnDisable()
    {
        renderer.enabled = false;
    }
}