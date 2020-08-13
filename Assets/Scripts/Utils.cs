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

    public static float RandomFloat(float min, float max)
    {
        var rnd = new Random();
        return (float) (min + rnd.NextDouble() / (max - min + 1));
    }
    
    public static int RandomInt(float min, float max)
    {
        var rnd = new Random();
        return Mathf.RoundToInt(min + rnd.Next() / (max - min + 1));
    }
}