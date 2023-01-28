using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryCameraController : MonoBehaviour
{
    
    [SerializeField] private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -10);
    }
}
