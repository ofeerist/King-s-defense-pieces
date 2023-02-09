using Assets.Scripts.Level;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Assets.Scripts.InspectorEditor
{
    [CustomEditor(typeof(GridCreator))]
    [CanEditMultipleObjects]
    public class GridCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var grid = (GridCreator)target;

            if (GUILayout.Button("Generate"))
            {
                grid.Create();
            }

            if (GUILayout.Button("Clear"))
            {
                grid.Clear();
                grid.Clear();
                grid.Clear();
                grid.Clear();
            }
        }
    }
}

#endif
