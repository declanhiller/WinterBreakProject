using System;
using UnityEngine;
public class GrappleHookController : MonoBehaviour {
    private KeybindController keybindController;
    private SpriteRenderer renderer;

    private void Start() {
        keybindController = GetComponentInParent<KeybindController>();
        renderer = GetComponent<SpriteRenderer>();
        Disable();
    }

    public void GrappleUpdate(Vector2 pos) {
        transform.position = pos;
    }

    public void Enable() {
        renderer.enabled = true;
    }

    public void Disable() {
        renderer.enabled = false;
    }
}