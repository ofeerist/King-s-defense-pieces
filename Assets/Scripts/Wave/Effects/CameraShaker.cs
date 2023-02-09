using Cinemachine;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Wave.Effects
{
    internal class CameraShaker : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _amplitude;
        [SerializeField] private float _shakeTimeout;

        private CinemachineBasicMultiChannelPerlin _cameraShake;
        private readonly SerialDisposable _cameraShakeDisposable = new();

        private void Start()
        {
            _cameraShake = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _cameraShakeDisposable.AddTo(this);
        }

        public void Shake()
        {
            _cameraShake.m_AmplitudeGain = _amplitude;

            _cameraShakeDisposable.Disposable =
                Observable.Timer(System.TimeSpan.FromSeconds(_shakeTimeout)).Subscribe(x =>
                {
                    _cameraShake.m_AmplitudeGain = 0;
                });
        }
    }
}
