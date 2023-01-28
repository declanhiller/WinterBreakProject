using System;
using System.Collections.Generic;
using UnityEngine;

namespace V2.Scripts.Player
{
    public class PlayerMoveController : MonoBehaviour
    {

        private Rigidbody2D rb;
        
        
        private PlayerState currentState;

        public PlayerState GroundedState { get; private set; }
        public PlayerState InAirState { get; private set; }

        public PersistentPlayerData persistentData;
        

        private void Update()
        {


            CurrentFrameData currentFrameData = CreateCurrentFrameData();
            

            currentState.Tick(currentFrameData);
        }

        private CurrentFrameData CreateCurrentFrameData()
        {
            throw new NotImplementedException();
        }

        private void FixedUpdate()
        {
            
        }
        
        //set to input action like normal


        public void ChangeState(PlayerState nextState)
        {
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
        }
    }
}