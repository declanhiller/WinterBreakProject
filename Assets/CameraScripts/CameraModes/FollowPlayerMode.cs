
using UnityEngine;

public class FollowPlayerMode : ICameraMode
{
    private float lerpValue = 0.5f;
    
    public Vector2 Update(CameraController controller)
    {
        Vector2 playerPosition = controller.GetPlayerPos();
        Vector3 targetPos = new Vector3(playerPosition.x, playerPosition.y);
        Vector3 actualWantedPos = Vector3.Lerp(controller.GetCameraPos(), targetPos, lerpValue);
        return actualWantedPos;
    }
}