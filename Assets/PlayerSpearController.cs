using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpearController : MonoBehaviour, IPlayerFunctionController
{
    [Header("Attack Settings")]
    [SerializeField] private Hitbox[] hitboxes;
    
    
    [Header("Debug Mode")] [SerializeField]
    private bool debugMode;
    
    
    private PlayerController playerController;

    private int seqeunceNumber;

    private Collider2D currentActiveHitbox;
    
    private void Awake()
    {
        foreach (Hitbox hitbox in hitboxes)
        {
            hitbox.collider.enabled = false;
        }
        playerController = GetComponentInParent<PlayerController>();
        playerController.AddFunction(this);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        Hitbox hitbox = hitboxes[seqeunceNumber];
        hitbox.collider.enabled = true;
        currentActiveHitbox = hitbox.collider;
        Invoke(nameof(DeactivateHitbox), hitbox.hitboxTimer);
        
        seqeunceNumber++;
        if (seqeunceNumber >= hitboxes.Length)
        {
            seqeunceNumber = 0;
        }
    }

    public void DeactivateHitbox()
    {
        currentActiveHitbox.enabled = false;
        currentActiveHitbox = null;
    }

    public void RestoreToRestState()
    {
        
    }


    public void Tick()
    {
        
    }

    public void SetKeybinds(KeybindController keybindController)
    {
        keybindController.SetStartAttackListener(Attack);
    }

    public void ReceiveMessage(string msg)
    {
        
    }

    public int GetPriority()
    {
        return 3;
    }


    [Serializable] struct Hitbox
    {
        public Collider2D collider;
        public float hitboxTimer;
    }
}