using Assets.Scripts.States;
using System;

namespace Assets.Scripts.UI.Cards.States
{
    public class BaseCardAnimationState : State
    {

        public event Action OnStateInitialize;

        public bool IsInitialized { get; private set; }

        public void Initialize(CardStateAnimator animator)
        {
            OnStateInitialize?.Invoke();

            OnInitialize(animator);

            IsInitialized = true;
        }

        protected virtual void OnInitialize(CardStateAnimator animator)
        {

        }
    }
}
