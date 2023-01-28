using UnityEngine;

namespace V2.Scripts.Player
{
    public class JumpProcedure
    {

        public Rigidbody2D rb;
        public PersistentPlayerData persistentData;
        public bool hasTriggered;
        
        public void StartJump()
        {
            hasTriggered = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * persistentData.jumpForce * 10);
        }

        public void EndJump()
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }

        public void ResetTrigger()
        {
            hasTriggered = false;
        }
    }
}