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

        private ISelectable _currentSelectable;

        private void Start()
        {
            var results = new List<RaycastResult>();
            var eventSystem = EventSystem.current;

            Observable.EveryUpdate().Subscribe(x =>
            {
                results.Clear();

                var pointerData = new PointerEventData(eventSystem)
                {
                    position = _mouseAction.ReadValue<Vector2>()
                };

                _raycaster.Raycast(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    var resultObject = result.gameObject;

                    if ((_layerMask & (1 << resultObject.layer)) == 0 
                    || !resultObject.TryGetComponent<ISelectable>(out var selectable))
                        continue;
                    
                    DeselectCurrent();

                    selectable.Select();
                    _currentSelectable = selectable;

                    return;
                }

                DeselectCurrent();
            }).AddTo(this);

            _mouseAction.Enable();
        }

        private void DeselectCurrent()
        {
            if (_currentSelectable == null) return;

            _currentSelectable.Deselect();
            _currentSelectable = null;
        }
    }
}
