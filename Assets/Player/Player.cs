using System;
using Player.Spear;
using UnityEngine;

namespace Player {
    public class Player : MonoBehaviour {
        public ISpearState SpearState { get; private set; }

        [SerializeField] public SpearStowedState stowedState;
        [SerializeField] public SpearAimingState aimingState;
        [SerializeField] public SpearAttackState attackState;

        public KeybindController KeybindController { get; private set;}

        private void Awake() {
            KeybindController = GetComponent<KeybindController>();
            // stowedState = new SpearStowedState(this);
            // aimingState = new SpearAimingState(this);
            // attackState = new SpearAttackState(this);
        }

        private void Update() {
            ISpearUpdatable updatable = SpearState as ISpearUpdatable;
            updatable?.Update();
        }

        public void ChangeSpearState(ISpearState state) {
            SpearState?.End();
            SpearState = state;
            SpearState?.Start();
        }
    }
}