using System;
using UnityEngine.InputSystem;

namespace Player.Spear {
    [Serializable]
    public class SpearStowedState : ISpearState {
        private Player player;

        public SpearStowedState(Player player) {
            this.player = player;
            player.KeybindController.AddAttackStartListener(OnStartAttack);
            player.KeybindController.AddStartGrappleListener(OnStartGrapple);
        }
        

        public void End() {
            
        }

        public void Start() {
            
        }

        public void OnStartAttack(InputAction.CallbackContext context) {
            if (player.SpearState != this) return;
            player.ChangeSpearState(player.attackState);
        }

        public void OnStartGrapple(InputAction.CallbackContext context) {
            if (player.SpearState != this) return;
            player.ChangeSpearState(player.aimingState);
        }
    }
}