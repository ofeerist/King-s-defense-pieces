using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Level
{
    [ExecuteInEditMode]
    public class ReplaceWithPrefab : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _prefab;

        public void Replace()
        {
            var toDestroy = new List<GameObject>();
            var childCount = _parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = _parent.GetChild(i);

                var newChild = PrefabUtility.InstantiatePrefab(_prefab, _parent) as GameObject;
                newChild.name += " " + i.ToString();
                newChild.transform.position = child.transform.position;

                toDestroy.Add(child.gameObject);
            }

            foreach (var item in toDestroy)
            {
                DestroyImmediate(item);
            }
        }
    }
}
