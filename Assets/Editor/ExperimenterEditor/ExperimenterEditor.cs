using UnityEditor;
using UnityEngine;
using Experimenter;

namespace Editor.ExperimenterEditor
{
    [CustomEditor(typeof(Experimenter.Experimenter))]
    public class ExperimenterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var exp = (Experimenter.Experimenter) target;
            DrawDefaultInspector();

            if (GUILayout.Button("Start"))
            {
                exp.Start();
            }
        }
    }
}