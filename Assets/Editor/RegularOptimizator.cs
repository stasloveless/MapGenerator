using UnityEngine;
using UnityEditor;

namespace Editor
{
    public class RegularOptimizator
    {
        private static int levelOfdetail = 0;

        public static void Draw()
        {
            levelOfdetail = Mathf.RoundToInt(EditorGUILayout.Slider("Level of detail", levelOfdetail, 1, 10));
        }
    }
}