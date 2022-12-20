
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleControllerV2 : MonoBehaviour
{
    
    [SerializeField] private float slowedDownTime = 0f;
    [SerializeField] private float timeRate = 0.1f;
    [SerializeField] private float grappleTimeLimit = 1f;
    [SerializeField] private float spriteScale = 1f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask grapplePointMask;
    [SerializeField] private float grappleSpeed = 10f;
    
    private float defaultTimeScale;
    private float fixedDeltaTime;
    
    
    [SerializeField] private GrappleBoundary boundary;
    [SerializeField] private GrappleAimer grappleAimer;
    [SerializeField] private Grapple grapple;
    
    private KeybindController keybindController;

    private GrappleState state;
    
    private Coroutine slowDownCoroutine;

    
    
    private void Awake()
    {
        keybindController = GetComponentInParent<KeybindController>();
    }

    private void Start()
    {
        keybindController.SetStartGrappleListener(StartAiming);
        keybindController.SetEndGrappleListener(ShootOut);
        boundary.enabled = false;
        grappleAimer.enabled = false;
        grapple.enabled = false;
        defaultTimeScale = Time.timeScale;
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        switch (state)
        {
            //should not have to write child specific update code in here...
            //I think it should go in it's own update function????
            case GrappleState.STOWED:
                break;
            case GrappleState.AIMING:
                AimingUpdate();
                break;
            case GrappleState.SHOOTING:
                
                break;
            case GrappleState.CONNECTED:
                break;
        }
    }

    private void StartAiming(InputAction.CallbackContext context)
    {
        grappleAimer.enabled = true;
        boundary.enabled = true;
        state = GrappleState.AIMING;
        slowDownCoroutine = StartCoroutine(SlowDownTime(slowedDownTime));
    }

    private void AimingUpdate()
    {
        Vector2 mousePos = keybindController.ReadMouseValueInWorld();
        Vector2 startPointPos = transform.position;
        grappleAimer.UpdateCrosshair(mousePos);
        boundary.RenderCircle(radius, startPointPos);
    }

    private void ShootOut(InputAction.CallbackContext context)
    {
        RestoreTime();
        grappleAimer.enabled = false;
        boundary.enabled = false;
        state = GrappleState.SHOOTING;
        grapple.enabled = true;
        grapple.StartGrappleShoot(grappleAimer.GetDirection(), radius);
        grapple.SetOnGrapple(OnGrapple);
    }

    private void OnGrapple()
    {
        state = GrappleState.CONNECTED;
    }
    
    
    IEnumerator SpeedUpTime(float targetTime) {
        float time = Time.timeScale;
        while (time < targetTime) {
            Time.timeScale += timeRate * Time.deltaTime * 5;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            time = Time.timeScale;
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = targetTime;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }

    IEnumerator SlowDownTime(float targetTime) {

        float time = Time.timeScale;

        while (time > targetTime) {
            Time.timeScale -= timeRate * Time.deltaTime;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            time = Time.timeScale;
            yield return new WaitForEndOfFrame();
        }
        
        Time.timeScale = targetTime;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
    
    private void RestoreTime() {
        if (slowDownCoroutine != null) {
            StopCoroutine(slowDownCoroutine);
        }

        StartCoroutine(SpeedUpTime(defaultTimeScale));
    }
}