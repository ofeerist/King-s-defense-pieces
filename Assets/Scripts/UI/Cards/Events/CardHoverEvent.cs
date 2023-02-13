using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Cards.Events
{
    internal class CardHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CardStateAnimator _animator;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _animator.ChangeState(_animator.AnimationStates.HoverState);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.ChangeState(_animator.AnimationStates.StillState);
        }
    }
}
