using System;

namespace Player.Spear {
    [Serializable]
    public class SpearAimingState : ISpearState, ISpearUpdatable {
        private Player player;
        
        public SpearAimingState(Player player) {
            this.player = player;
        }



        public void End() {
            
        }

        public void Start() {
            
        }

        public void Update() {
            
        }
    }
}