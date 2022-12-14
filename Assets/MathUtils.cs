using System;
using UnityEngine;

public class MathUtils {
    public static float SquaredDistance(Vector2 startPos, Vector2 endPos) {
        return (float) (Math.Pow(endPos.x - startPos.x, 2) + Math.Pow(endPos.y - startPos.y, 2));
    }
}