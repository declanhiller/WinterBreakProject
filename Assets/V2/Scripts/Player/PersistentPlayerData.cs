using UnityEngine;

namespace V2.Scripts.Player
{
    public class PersistentPlayerData : ScriptableObject
    {

        
        [Header("Move Values")]
        [SerializeField] public float acceleration = 100f;
        [SerializeField] public float maxMoveSpeed = 10;

        [Header("Jump Values")] 
        [SerializeField] public float jumpForce = 130;
        [SerializeField] public float jumpCoyoteTime = 0.07f;

        [Header("Collision Values")]
        [SerializeField] public LayerMask groundMask;
        [SerializeField] public Transform playerFeet;
        [SerializeField] public float groundDetectionPrecision = 0.05f;

    }
}