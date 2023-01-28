using Unity.VisualScripting;
using UnityEngine;

namespace V2.Scripts.Player
{
    public class GroundedState : PlayerState
    {
        






        private Vector2 directionOfSlopePlayerIsOn;
        
        public GroundedState(PlayerComponents components, PersistentPlayerData persistentData) : base(components, persistentData)
        {
        }
        

        public override void Enter()
        {
            //set correct rb values...
            //set listeners...

            components.jumpProcedure.hasTriggered = false;
            components.jumpProcedure.ResetTrigger();
        }

        protected override void LogicTick()
        {
            Move();
            CheckJump();
            RaycastHit2D raycastHit2D = Physics2D.Raycast(persistentData.playerFeet.position, Vector2.down, persistentData.groundDetectionPrecision, persistentData.groundMask);
            if (raycastHit2D.collider == null)
            {
                components.moveController.ChangeState(components.moveController.InAirState);
                return;
            }
            RotateNormals();
        }

        private void CheckJump()
        {
            if (!currentFrameData.startedJump) return;
            if (components.jumpProcedure.hasTriggered) return;
            components.jumpProcedure.StartJump();
        }


        void RotateNormals()
        {
            RaycastHit2D hit = Physics2D.Raycast(persistentData.playerFeet.position, Vector2.down, 0.2f, persistentData.groundMask);
            if (hit.collider == null) return;
            //set for use by move method
            directionOfSlopePlayerIsOn = -Vector2.Perpendicular(hit.normal).normalized; 
            
            //set player normals so they are rotated towards slope of field
            components.transform.rotation = Quaternion.FromToRotation(components.transform.up, hit.normal) * components.transform.rotation;
        }

        public void Move()
        {
            float currentSpeed = components.rb.velocity.magnitude;

            //flip speed magnitude so it points the way the player is going on the x axis
            if (components.rb.velocity.x < 0)
            {
                currentSpeed *= -1;
            }
            
            currentSpeed += currentFrameData.xRange * persistentData.acceleration * Time.deltaTime * 10;
            currentSpeed = Mathf.Clamp(currentSpeed, -persistentData.maxMoveSpeed, persistentData.maxMoveSpeed);

            bool isFullStopping = currentFrameData.xRange == 0 && currentSpeed != 0;

            float direction = 0;
            if (currentFrameData.xRange > 0)
            {
                direction = 1;
            } else if(currentFrameData.xRange < 0) {
                direction = -1;
            }

            if (isFullStopping)
            {
                currentSpeed -= direction * persistentData.acceleration * Time.deltaTime;
                if (currentSpeed <= 0 && direction > 0 || currentSpeed >= 0 && direction < 0) //Make sure player comes to full stops when doing gradual stops
                {
                    currentSpeed = 0;
                }
            } else if (currentFrameData.xRange > 0 && direction < 0 || currentFrameData.xRange < 0 && direction > 0) //let player change directions on a dime instead of slowly adding force in the other direction
            {
                currentSpeed = 0;
                currentSpeed += currentFrameData.xRange * persistentData.acceleration * Time.deltaTime * 10;
                direction = currentFrameData.xRange > 0 ? 1 : -1;
            }
            
            
            
        }
        
        

        public override void Exit()
        {
            //turn off even listeners
        }
        
    }
}