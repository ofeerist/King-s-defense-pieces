using Assets.Scripts.Level;
using Cinemachine;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Wave
{
    internal class PawnFactory : MonoBehaviour
    {
        [SerializeField] private PawnAnimator _pawn;

        [SerializeField] private float _speed;
        public float Speed { get { return _speed; } set { _speed = value; } }

        [SerializeField] private KeyPoint[] _keyPoints;

        [SerializeField] private int _pawnLimit;
        private int _pawnCount;

        [SerializeField] private float _tick;

        private readonly List<PawnAnimator> _pawns = new();

        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _amplitude;
        [SerializeField] private float _shakeTimeout;
        private CinemachineBasicMultiChannelPerlin _cameraShake;
        private readonly SerialDisposable _cameraShakeDisposable = new();

        private void Start()
        {
            _cameraShake = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            Observable.Interval(System.TimeSpan.FromSeconds(_tick)).Subscribe(x =>
            {
                CreatePawn();

                foreach (var pawn in _pawns)
                {
                    MoveNextPawn(pawn);
                }
            }).AddTo(this);

            _cameraShakeDisposable.AddTo(this);
        }

        private void Pawn_OnAnimationFinished(PawnAnimator pawn)
        {
            _cameraShake.m_AmplitudeGain = _amplitude;

            _cameraShakeDisposable.Disposable =
                Observable.Timer(System.TimeSpan.FromSeconds(_shakeTimeout)).Subscribe(x =>
            {
                _cameraShake.m_AmplitudeGain = 0;
            });

            _keyPoints[pawn.KeyID].PlayParticle();
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
            var pawn = Instantiate(_pawn, start.position, Quaternion.identity);
            pawn.SetFactory(this);

            pawn.OnAnimationFinished += Pawn_OnAnimationFinished;

            _pawns.Add(pawn);
        }
    }
}
