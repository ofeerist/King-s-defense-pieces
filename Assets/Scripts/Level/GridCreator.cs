using UnityEngine;

namespace Assets.Scripts.Level
{
    public class GridCreator : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        [Space]

        [SerializeField] private Vector2 _size;
        [SerializeField] private Vector3 _offset;

        public void Create()
        {
            var _transform = transform;
            var position = _transform.position;

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    var obj = Instantiate(_prefab, _transform);
                    obj.transform.position = MultiplyVectorCoordinates(
                        new Vector3(x, position.y, y), _offset);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < transform.childCount; i++)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        private Vector3 MultiplyVectorCoordinates(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }
}
