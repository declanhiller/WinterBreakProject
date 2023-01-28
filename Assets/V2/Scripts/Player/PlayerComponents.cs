using UnityEngine;

namespace V2.Scripts.Player
{
    public class PlayerComponents
    {
        public Rigidbody2D rb { get; set; }
        
        public Transform transform { get; set; }
        
        public PlayerMoveController moveController { get; set; }
        
        public JumpProcedure jumpProcedure { get; set; }
    }
}