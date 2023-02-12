using Assets.Scripts.UI.Selectables.Cards;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Building
{
    internal class BuildRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask _buildMask;

        [SerializeField] private Camera _camera;
        [SerializeField] private InputAction _buildAction;
        [SerializeField] private InputAction _mousePositionAction;

        [SerializeField] private RectTransform _rectTransform;

        private RaycastHit _result = new();

        private void Start()
        {
            _buildAction.Enable();
            _mousePositionAction.Enable();
        }

        public bool TryRaycastBuildCell(Ray ray, out BuildCell buildCell)
        {
            buildCell = null;

            var raycast = Physics.Raycast(ray, out _result, Mathf.Infinity, _buildMask);

            if (!raycast) return false;

            if (_result.collider.TryGetComponent(out buildCell))
            {
                return true;
            }

            return false;
        }

        public bool TryRaycastBuildCellPosition(Vector2 screenPoint, out Vector3 outPosition, out BuildCell cell)
        {
            outPosition = Vector3.zero;

            var ray = _camera.ScreenPointToRay(screenPoint);

            if (TryRaycastBuildCell(ray, out cell)) 
            {
                outPosition = CameraWorldToScreenPoint(cell.transform.position);

                return true;
            }

            return false;
        }

        public Vector3 CameraWorldToScreenPoint(Vector3 position)
        {
            return _camera.WorldToScreenPoint(position);
        }
    }
}
