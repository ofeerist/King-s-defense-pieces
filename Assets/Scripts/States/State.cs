using System;
using UnityEngine;

namespace Assets.Scripts.States
{
    [Serializable]
    public class State 
    {
        public event Action OnStateEnter;
        public event Action OnStateExit;

        public void Enter()
        {
            OnStateEnter?.Invoke();

            OnEnter();
        }

        protected virtual void OnEnter()
        {

        }

        public void Exit()
        {
            OnStateExit?.Invoke();

            OnExit();
        }

        protected virtual void OnExit()
        {

        }
    }
}