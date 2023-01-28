namespace V2.Scripts.Player
{
    public class PersistentPlayerAction
    {
        public delegate void Tick(CurrentFrameData frameData);

        public PersistentActionSignature signature;
    }
}