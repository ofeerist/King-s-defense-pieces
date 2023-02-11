using Assets.Scripts.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Selectables.Cards
{
    internal class CardDragNDrop : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private BuildRaycaster _buildRaycaster;

        [Space]

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardAnimator _cardAnimator;
        [SerializeField] private LayoutElement _layoutElement;

        public void OnDrag(PointerEventData eventData)
        {
            _cardAnimator.IsHand = false;

            _rectTransform.position = _buildRaycaster.RaycastBlock(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _cardAnimator.IsHand = true;

            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
