using UnityEngine.InputSystem;

namespace V2.Scripts.Player
{
    public abstract class PlayerState
    {
        protected PlayerComponents components;
        protected PersistentPlayerData persistentData;

        protected CurrentFrameData currentFrameData;
        

        protected PlayerState(PlayerComponents components, PersistentPlayerData persistentData)
        {
            this.components = components;
            this.persistentData = persistentData;
        }

        public abstract void Enter();

        public void Tick(CurrentFrameData frameData)
        {
            currentFrameData = frameData;
            LogicTick();
        }

        protected abstract void LogicTick();
        
        public abstract void Exit();
    }
}