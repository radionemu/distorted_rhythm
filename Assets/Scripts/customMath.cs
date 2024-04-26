using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CustomMath : MonoBehaviour
{
    public static Vector2 ExtraLerp(Vector2 a, Vector2 b, float t){
        return a + (b-a)*t;
    }

    public static Vector3 ExtraLerp(Vector3 a, Vector3 b, float t){
        return a + (b-a)*t;
    }
}
