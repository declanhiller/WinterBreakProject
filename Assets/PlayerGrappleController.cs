
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrappleController : MonoBehaviour
{

    private PlayerController playerController;
    
    [SerializeField] private float slowedDownTime = 0f;
    [SerializeField] private float timeRate = 0.1f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask grapplePointMask;
    [SerializeField] private float grappleSpeed = 10f;
    
    private float defaultTimeScale;
    private float fixedDeltaTime;
    
    
    // [SerializeField] private GrappleBoundary boundary;
    // [SerializeField] private GrappleAimer grappleAimer;
    // [SerializeField] private Grapple grapple;
    
    private KeybindController keybindController;

    private GrappleState state;
    
    private Coroutine slowDownCoroutine;

    public const string GRAPPLE_STARTED = "GRAPPLE_STARTED";
    public const string GRAPPLE_ENDED = "GRAPPLE_ENDED";
    
    
    private void Awake()
    {
        keybindController = GetComponentInParent<KeybindController>();
    }

    private void Start()
    {
        keybindController.AddStartGrappleListener(StartGrappleAction);
        keybindController.AddEndGrappleListener(EndGrappleAction);
        // boundary.enabled = false;
        // grappleAimer.enabled = false;
        // grapple.enabled = false;
        defaultTimeScale = Time.timeScale;
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {

        // if (grapple.DidHookConnect())
        // {
        //     state = GrappleState.CONNECTED;
        // }
        
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

    private void StartGrappleAction(InputAction.CallbackContext context)
    {
        if (state == GrappleState.STOWED)
        {
            // grappleAimer.enabled = true;
            // boundary.enabled = true;
            state = GrappleState.AIMING;
            slowDownCoroutine = StartCoroutine(SlowDownTime(slowedDownTime));
            playerController.SendMsg(GRAPPLE_STARTED);
        }
        else if (state == GrappleState.CONNECTED)
        {
            // grapple.enabled = false;
            state = GrappleState.STOWED;
            playerController.SendMsg(GRAPPLE_ENDED);
        }
    }

    private void AimingUpdate()
    {
        Vector2 mousePos = keybindController.ReadMouseValueInWorld();
        Vector2 startPointPos = transform.position;
        // grappleAimer.UpdateCrosshair(mousePos);
        // boundary.RenderCircle(radius, startPointPos);
    }

    private void EndGrappleAction(InputAction.CallbackContext context)
    {
        if (state == GrappleState.AIMING)
        {
            RestoreTime();
            // grappleAimer.enabled = false; 
            // boundary.enabled = false;
            state = GrappleState.SHOOTING;
            // grapple.enabled = true;
            // grapple.StartGrappleShoot(grappleAimer.GetDirection(), radius);
            // grapple.SetOnGrapple(OnGrapple);
        }
    }

    private void OnGrapple()
    {
        state = GrappleState.SHOOTING;
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