using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class KeyPointsSorter : MonoBehaviour
    {
        [SerializeField] private List<KeyPoint> _keyPoints;

        [SerializeField] private KeyPoint _start;
        [SerializeField] private float _radius;

        [SerializeField] private List<KeyPoint> _sorted = new();

        public void Sort()
        {
            _sorted.Clear();

            _sorted.Add(_start);
            FindKey(_start.position);
        }

        private void FindKey(Vector3 position)
        {
            var results = Physics.OverlapSphere(position, _radius);
            var orderedByProximity = results.OrderBy(c => (position - c.transform.position).sqrMagnitude).ToArray();

            foreach (var item in orderedByProximity)
            { 
                if (!item.gameObject.TryGetComponent<KeyPoint>(out var keyPoint))
                    continue;

                if (_sorted.Contains(keyPoint))
                    continue;

                _sorted.Add(keyPoint);
                FindKey(keyPoint.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_start.position, _radius);
        }
    }
}
