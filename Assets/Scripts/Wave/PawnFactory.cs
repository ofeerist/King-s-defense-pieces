using Assets.Scripts.Level;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Wave
{
    internal class PawnFactory : MonoBehaviour
    {
        [SerializeField] private PawnAnimator _pawnPrefab;

        [SerializeField] private float _speed;
        public float Speed { get { return _speed; } set { _speed = value; } }

        [SerializeField] private KeyPoint[] _keyPoints;
        public KeyPoint[] KeyPoints => _keyPoints;

        [SerializeField] private int _pawnLimit;
        private int _pawnCount;

        private readonly List<PawnAnimator> _pawns = new();

        public UnityEvent<PawnAnimator> OnAnimationFinished;

        public void Tick()
        {
            CreatePawn();

            foreach (var pawn in _pawns)
            {
                MoveNextPawn(pawn);
            }
        }

        private void Pawn_OnAnimationFinished(PawnAnimator pawn)
        {
            OnAnimationFinished?.Invoke(pawn);
        }

        private void MoveNextPawn(PawnAnimator pawn)
        {
            var nextId = pawn.KeyID + 1;

            if (nextId >= _keyPoints.Length) return;

            pawn.Move(_keyPoints[nextId].position, nextId);
        }

        private void CreatePawn()
        {
            if (_pawnCount >= _pawnLimit) return;

            _pawnCount++;

            var start = _keyPoints[0];
            var pawn = Instantiate(_pawnPrefab, start.position, Quaternion.identity);
            pawn.SetFactory(this);

            pawn.OnAnimationFinished += Pawn_OnAnimationFinished;

            _pawns.Add(pawn);
        }
    }
}
