using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleController : MonoBehaviour {

    private PlayerController playerController;
    private KeybindController keybindController;
    
    private GrappleState state;

    [SerializeField] private Transform hook;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private LineRenderer lineRenderer;

    
    private void Awake() {
        state = GrappleState.STOWED;
        keybindController = GetComponentInParent<KeybindController>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Start() {
        hook.gameObject.SetActive(false);
        keybindController.AddStartGrappleListener(StartGrappleAction);
        keybindController.AddEndGrappleListener(EndGrappleAction);
    }


    private void Update() {
        if (hook.gameObject.activeInHierarchy) {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hook.position);
        }

    }

    private void StartGrappleAction(InputAction.CallbackContext context)
    {
        if (state == GrappleState.STOWED)
        {
            state = GrappleState.SHOOTING;
            ShootOutGrapple();
        }
        else if (state == GrappleState.CONNECTED)
        {
            state = GrappleState.STOWED;
        }
    }

    private void ShootOutGrapple() {
        hook.gameObject.SetActive(true);
        hook.transform.position = transform.position;
        Vector2 readMouseValueInWorld = keybindController.ReadMouseValueInWorld();
        StartCoroutine(MoveTo((readMouseValueInWorld)));
    }

    IEnumerator MoveTo(Vector3 targetPosition) {
        while (hook.position != targetPosition) {
            Vector3 nextPos = Vector3.MoveTowards(hook.position, targetPosition, 25 * Time.deltaTime);
            hook.position = nextPos;
            yield return new WaitForEndOfFrame();
        }

        Collider2D overlapCircle = Physics2D.OverlapCircle(targetPosition, 0.01f);
        if (overlapCircle == null) yield break;

        rb.velocity = Vector2.zero;

        Vector2 direction = (hook.position - transform.position).normalized;

        rb.AddForce(direction * 2000);
    }

    private void EndGrappleAction(InputAction.CallbackContext context)
    {
        if (state == GrappleState.AIMING)
        {
            state = GrappleState.SHOOTING;
        }
    }
}