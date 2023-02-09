using Assets.Scripts.Level;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Assets.Scripts.InspectorEditor
{
    [CustomEditor(typeof(ReplaceWithPrefab))]
    [CanEditMultipleObjects]
    internal class ReplaceWithPrefabEdtior : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var replacer = (ReplaceWithPrefab)target;

            if (GUILayout.Button("Replace"))
            {
                replacer.Replace();
            }
        }
    }
}

#endif