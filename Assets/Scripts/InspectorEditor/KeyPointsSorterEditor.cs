using Assets.Scripts.Level;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Assets.Scripts.InspectorEditor
{
    [CustomEditor(typeof(KeyPointsSorter))]
    [CanEditMultipleObjects]
    public class KeyPointsSorterEdtior : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var sorter = (KeyPointsSorter)target;

            if (GUILayout.Button("Sort"))
            {
                sorter.Sort();
            }
        }
    }
}

#endif
