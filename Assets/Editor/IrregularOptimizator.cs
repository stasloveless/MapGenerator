using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class IrregularOptimizator
    {
        private static float epsilon = 0.1f;
        private static bool usePseudoDistance = false;
        private static float constantC = 0.1f;
        
        public static void Draw()
        {
            epsilon = EditorGUILayout.Slider("Ramer-Duglas E",epsilon, 0.01f, 1);
            usePseudoDistance = EditorGUILayout.Toggle("Use pseudo distance", usePseudoDistance);

            if (usePseudoDistance)
            {
                constantC = EditorGUILayout.Slider("Constant C",constantC, 0, 1);
            }
            
        }
    }
}