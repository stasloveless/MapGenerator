using System;
using UnityEngine;
using Random = System.Random;

public static class Utils
{
    public static Vector3 ChangeX(Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }
        
    public static Vector3 ChangeY(Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }
        
    public static Vector3 ChangeZ(Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static float RandomFloat(int min, int max, Random rnd)
    {
        return rnd.Next(min * 10, max * 10) / 10.0f;
    }
}