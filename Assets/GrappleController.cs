using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleController : MonoBehaviour {
    [SerializeField] private float slowedDownTime = 0f;
    [SerializeField] private float timeRate = 0.1f;
    [SerializeField] private float grappleTimeLimit = 1f;
    [SerializeField] private float spriteScale = 1f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask grapplePointMask;
    [SerializeField] private float grappleSpeed = 10f;
    
    private float defaultTimeScale;
    private float fixedDeltaTime;

    private Coroutine slowDownCoroutine;
    
    private KeybindController keybindController;

    private LineRenderer renderer;

    private GrappleHookController endController;
    private GrappleBoundaryController boundaryController;
    private GrappleAimer grappleAimer;
    
    private bool isGoingForGrapple;
    private bool isGrappling;
    private bool isGrappled;

    private float currentGrappleLength;
    private Vector2 directionOfGrapple;
    private Vector2 currentGrappleHookPosition;
    private void Awake() {
        // Cursor.visible = false;
        keybindController = GetComponentInParent<KeybindController>();
        renderer = GetComponent<LineRenderer>();
        renderer.enabled = false;
    }

    private void Start() {
        keybindController.SetStartGrappleListener(StartGrapple);
        keybindController.SetEndGrappleListener(EndGrapple);
        defaultTimeScale = Time.timeScale;
        this.fixedDeltaTime = Time.fixedDeltaTime;
        endController = GetComponentInChildren<GrappleHookController>();
        boundaryController = GetComponentInChildren<GrappleBoundaryController>();
        grappleAimer = GetComponentInChildren<GrappleAimer>();
    }

    private void Update() {
        if (isGoingForGrapple) {
            Vector2 mousePos = keybindController.ReadMouseValueInWorld();
            Vector2 startPointPos = transform.position;
            directionOfGrapple = (mousePos - startPointPos).normalized;

            grappleAimer.Enable();
            grappleAimer.UpdateCrosshair(mousePos);
            boundaryController.RenderCircle(radius, startPointPos);
            

        } else if (isGrappling) {
            grappleAimer.Disable();
            Vector2 increase = directionOfGrapple * grappleSpeed * Time.deltaTime;
            currentGrappleHookPosition += increase;
            renderer.SetPosition(0, transform.position);
            renderer.SetPosition(1, currentGrappleHookPosition);
            endController.GrappleUpdate(currentGrappleHookPosition);
            if (MathUtils.SquaredDistance(transform.position, currentGrappleHookPosition) >= radius * radius) {
                isGrappling = false;
                renderer.positionCount = 0;
                renderer.enabled = false;
                endController.Disable();
            }

            RaycastHit2D raycastHit2D = Physics2D.CircleCast(currentGrappleHookPosition, 0.1f, Vector2.zero, 0, grapplePointMask);

            if (raycastHit2D.collider != null)
            {
                isGrappling = false;
                isGrappled = true;
            }
            
        } else if (isGrappled) {
            //keep track of stuff
            
        }
        else
        {
            renderer.positionCount = 0;
        }
    }

    private void StartGrapple(InputAction.CallbackContext context) {
        renderer.enabled = true;
        isGoingForGrapple = true;
        slowDownCoroutine = StartCoroutine(SlowDownTime(slowedDownTime));
        // StartCoroutine(GrappleTimer());
        endController.Enable();
        boundaryController.Enable();
    }

    private void EndGrapple(InputAction.CallbackContext context) {
        RestoreTime();
        ShootGrapple();
    }

    private void ShootGrapple() {
        renderer.enabled = true;
        renderer.positionCount = 2;
        isGrappling = true;
        currentGrappleLength = 0;
        currentGrappleHookPosition = transform.position;
        endController.Enable();
        grappleAimer.Disable();
    }

    IEnumerator GrappleTimer() {
        yield return new WaitForSecondsRealtime(grappleTimeLimit);
        RestoreTime();
    }

    private void RestoreTime() {
        if (slowDownCoroutine != null) {
            StopCoroutine(slowDownCoroutine);
        }

        StartCoroutine(SpeedUpTime(defaultTimeScale));

        renderer.enabled = false;
        isGoingForGrapple = false;
        endController.Disable();
        boundaryController.Disable();
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
}
