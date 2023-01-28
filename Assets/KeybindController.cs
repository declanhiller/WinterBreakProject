using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindController : MonoBehaviour {
    private PlayerKeybinds keybinds;
    private Camera camera;
    
    void Awake() {
        if (keybinds == null)
        {
            keybinds = new PlayerKeybinds();
        }
        keybinds.Enable();
        camera = Camera.main;
    }

    public float ReadRunValue() {
        return keybinds.Player.Move.ReadValue<float>();
    }

    public Vector2 ReadMouseValueInWorld() {
        return camera.ScreenToWorldPoint(keybinds.Player.MousePos.ReadValue<Vector2>());
    }

    public void AddStartGrappleListener(Action<InputAction.CallbackContext> grapplerListener) {
        keybinds.Player.Grapple.started += grapplerListener;
    }

    public void AddEndGrappleListener(Action<InputAction.CallbackContext> grapplerListener) {
        keybinds.Player.Grapple.canceled += grapplerListener;
    }

    public void AddAttackStartListener(Action<InputAction.CallbackContext> attackListener)
    {
        keybinds.Player.Attack.started += attackListener;
    }
    
    public void AddToJumpListener(Action<InputAction.CallbackContext> action) {
        keybinds.Player.Jump.started += action;
    }

    public void AddToJumpEndListener(Action<InputAction.CallbackContext> action)
    {
        keybinds.Player.Jump.canceled += action;
    }

    public void RemoveFromJumpListener(Action<InputAction.CallbackContext> action) {
        keybinds.Player.Jump.started -= action;
    }
}
