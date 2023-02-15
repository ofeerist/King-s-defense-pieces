using Assets.Scripts.States;
using Assets.Scripts.UI.Cards.States;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Cards
{
    [System.Serializable]
    public class AnimationLinks
    {
        public RectTransform CardsLayout;
        public GameObject CardsLayoutMask;
        public CardsLayout CardsLayoutSelectable;

        [Space]

        public RectTransform LayoutTransform;
        public LayoutElement LayoutLayoutElement;

        [Space]

        public RectTransform GraphicTransform;
    }

    [System.Serializable]
    public class AnimationStates
    {
        public BaseHandCardState StillState;
        public CardHoverState HoverState;
        public CardDragState DragState;
        public void InitializeStates(CardStateAnimator animator)
        {
            StillState.Initialize(animator);
            HoverState.Initialize(animator);
            DragState.Initialize(animator);
        }
    }

    public class CardStateAnimator : MonoBehaviour
    {
        public readonly StateMachine StateMachine = new();

        public State CurrentState => StateMachine.CurrentState;

        [SerializeField] private AnimationLinks _animationLinks;
        public AnimationLinks AnimationLinks { get => _animationLinks; }

        [SerializeField] private AnimationStates _animationStates;
        public AnimationStates AnimationStates { get => _animationStates; }

        private void Start()
        {
            _animationStates.InitializeStates(this);

            StateMachine.Initialize(_animationStates.StillState);
        }

        public void ChangeState(BaseCardAnimationState state)
        {
            StateMachine.ChangeState(state);
        }
    }
}
