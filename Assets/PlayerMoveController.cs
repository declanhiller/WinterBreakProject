using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 10f;


    private KeybindController keybindController;
    private Rigidbody2D rb;
    
    void Start() {
        keybindController = GetComponent<KeybindController>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        rb.velocity = new Vector2(keybindController.ReadRunValue() * moveSpeed, rb.velocity.y);
    }
}
