using System;

namespace Assets.Scripts.States
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public event Action<State, State> OnStateChanged;

        public void Initialize(State startingState)
        {
            CurrentState = startingState;

            startingState.Enter();
        }

        public void ChangeState(State newState)
        {
            var lastState = CurrentState;
            CurrentState.Exit();

            CurrentState = newState;
            newState.Enter();

            OnStateChanged?.Invoke(lastState, newState);
        }
    }
}
