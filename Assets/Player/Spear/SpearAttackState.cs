using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Spear {
    [Serializable]
    public class SpearAttackState : ISpearState, ISpearUpdatable {
        private Player player;
        [SerializeField] private float attackTime;

        private float timer;
        
        
        public SpearAttackState(Player player) {
            this.player = player;
        }
        
        
        public void Update() {
            while (timer > 0) {
                timer = attackTime;
            }
        }

        public void End() {
            
        }

        public void Start() {
            timer = attackTime;
        }

        public void OnGrapple(InputAction.CallbackContext context) {
            if (player.SpearState != this) return;
            player.ChangeSpearState(player.aimingState);
        }
        
    }
}