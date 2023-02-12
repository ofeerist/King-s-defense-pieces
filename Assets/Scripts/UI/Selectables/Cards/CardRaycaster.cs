using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UniRx;

namespace Assets.Scripts.UI.Selectables.Cards
{
    internal class CardRaycaster : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster _raycaster;

        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private InputAction _mouseAction;

        [SerializeField] private InputAction _selectAction;

        private bool _permantSelect;
        private ISelectable _currentSelectable;

        private readonly List<RaycastResult> _results = new();

        private void Start()
        {
            var eventSystem = EventSystem.current;

            Observable.EveryUpdate().Subscribe(x =>
            {
                if (_permantSelect) return;

                Deselect(_currentSelectable);

                if (TryRaycastSelectable(out var selectable))
                {
                    Select(selectable);
                }
            }).AddTo(this);

            _selectAction.performed += (arg) =>
            {
                if (TryRaycastSelectable(out var selectable))
                {
                    if (_permantSelect)
                    {
                        if (selectable == _currentSelectable)
                        {
                            _permantSelect = false;
                            Deselect(selectable);

                            return;
                        }
                        else
                        {
                            Deselect(_currentSelectable);

                            Select(selectable);
                        }
                    }

                    if (!_permantSelect)
                    {
                        _permantSelect = true;
                        Select(selectable);
                    }
                }
            };

            _mouseAction.Enable();
            _selectAction.Enable();
        }

        public bool TryRaycastSelectable(out ISelectable selectable)
        {
            _results.Clear();
            selectable = null;

            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = _mouseAction.ReadValue<Vector2>()
            };

            _raycaster.Raycast(pointerData, _results);

            foreach (RaycastResult result in _results)
            {
                var resultObject = result.gameObject;

                if ((_layerMask & (1 << resultObject.layer)) == 0
                || !resultObject.TryGetComponent(out selectable))
                    continue;

                return true;
            }

            return false;
        }

        private void Select(ISelectable selectable)
        {
            if (selectable == null) return;

            selectable.Select();
            _currentSelectable = selectable;
        }

        private void Deselect(ISelectable selectable)
        {
            if (selectable == null) return;

            selectable.Deselect();
            _currentSelectable = null;
        }
    }
}
