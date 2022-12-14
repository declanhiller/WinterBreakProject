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
    
    private bool isGoingForGrapple;
    private bool isGrappling;

    private float currentGrappleLength;
    private Vector2 directionOfGrapple;
    private Vector2 currentGrappleHookPosition;
    private void Awake() {
        Cursor.visible = false;
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
    }

    private void Update() {
        if (isGoingForGrapple) {
            Vector2 endPointPos = keybindController.ReadMouseValueInWorld();
            Vector2 startPointPos = transform.position;
            directionOfGrapple = (endPointPos - startPointPos).normalized;
            //Check for within radius
            float squaredDistance = MathUtils.SquaredDistance(startPointPos, endPointPos);
            RaycastHit2D hit = Physics2D.Raycast(startPointPos, directionOfGrapple, radius, grapplePointMask);
            if (hit.collider != null) {
                endPointPos = hit.point;
            } else if (squaredDistance > radius * radius) {
                endPointPos = directionOfGrapple * radius + startPointPos;

            }

            int spriteCount = Mathf.FloorToInt(Vector3.Distance(endPointPos, startPointPos) / spriteScale);
            Vector3[] positions = new Vector3[] {new Vector3(startPointPos.x, startPointPos.y, 5), new Vector3(endPointPos.x, endPointPos.y, 5)};
            renderer.positionCount = positions.Length;
            renderer.SetPositions(positions);
            if (renderer.material != null) {
                renderer.material.mainTextureScale = new Vector2(spriteScale * spriteCount, 1);
            }
            endController.GrappleUpdate(endPointPos);
            
            boundaryController.RenderCircle(radius, startPointPos);
            
        } else if (isGrappling) {
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
        } else {
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
