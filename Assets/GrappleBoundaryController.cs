using System;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleBoundaryController : MonoBehaviour {
    [SerializeField] private int segments = 360;
    private LineRenderer renderer;

    private void Start() {
        renderer = GetComponent<LineRenderer>();
    }
    
    public void RenderCircle(float radius, Vector2 startPointPos) {
        int pointCount = segments + 1;
        renderer.positionCount = pointCount;
        Vector3[] points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++) {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius) + new Vector3(startPointPos.x, startPointPos.y, 10);
        }
        renderer.SetPositions(points);
    }

    public void Enable() {
        renderer.enabled = true;
    }

    public void Disable() {
        renderer.enabled = false;
    }
}