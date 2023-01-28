using UnityEngine;

namespace V2.Scripts.Player
{
    public class CurrentFrameData
    {
        public bool startedJump { get; set; }
        public bool isJumping { get; set; }
        public bool endedJump { get; set; }
        
        public float xRange { get; set; }
        public bool didAttack { get; set; }
        
        
        
    }
}