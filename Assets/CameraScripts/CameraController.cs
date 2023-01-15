using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float lerpValue = 0.5f;
    [SerializeField] private CameraBounds[] cameraBounds;
    private CameraMode mode;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        CameraBounds bounds = null;
        foreach (CameraBounds o in cameraBounds)
        {
            if (o.bounds.Contains(playerTransform.position))
            {
                bounds = o;
                break;
            }
        }

        bounds?.cameraMode.Update(this);
    }

    public Vector2 GetPlayerPos()
    {
        return playerTransform.position;
    }

    public Vector2 GetCameraPos()
    {
        return transform.position;
    }
    
    [Serializable]
    public class CameraBounds
    {
        public ICameraMode cameraMode;
        public Bounds bounds;
    }

}

