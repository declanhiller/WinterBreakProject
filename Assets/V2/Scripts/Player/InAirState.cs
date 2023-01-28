using UnityEngine;

namespace V2.Scripts.Player
{
    public class InAirState : PlayerState
    {


        private float coyoteTimer;
        
        public InAirState(PlayerComponents components, PersistentPlayerData persistentData) : base(components, persistentData)
        {
        }

        public override void Enter()
        {
            
            coyoteTimer = 0;
        }

        protected override void LogicTick()
        {
            CheckJump();
            RaycastHit2D raycastHit2D = Physics2D.Raycast(persistentData.playerFeet.position, Vector2.down, persistentData.groundDetectionPrecision, persistentData.groundMask);
            if (raycastHit2D.collider != null)
            {
                components.moveController.ChangeState(components.moveController.GroundedState);
            }

        }

        private void CheckJump()
        {

            if (components.jumpProcedure.hasTriggered) return;
            if (coyoteTimer >= persistentData.jumpCoyoteTime) return;
            if (!currentFrameData.startedJump) return;
            
            components.jumpProcedure.StartJump();
            
            
            coyoteTimer += Time.deltaTime;
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
        
        //Air states, jump states, 
        
        
        
    }
}