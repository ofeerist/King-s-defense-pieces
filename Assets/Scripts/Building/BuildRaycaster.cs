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
        private void Start()
        {
            _buildAction.Enable();
            _mousePositionAction.Enable();

            _buildAction.performed += (arg) => Build(null);
        }

        public void Build(Card card)
        {
            var ray = _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>());
            var results = Physics.RaycastAll(ray, Mathf.Infinity, _buildMask);
            var orderedByProximity = results.OrderBy(c => (_camera.transform.position - c.transform.position).sqrMagnitude).ToArray();

            if (orderedByProximity.Length == 0) return;

            _rectTransform.position = _camera.WorldToScreenPoint(orderedByProximity[0].collider.gameObject.transform.position);
        }
    }
}
