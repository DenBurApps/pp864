using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Stove
{
    public class HeatMeter : MonoBehaviour
    {
        [SerializeField] private Image _heatMeter;
        [SerializeField] private Image _potImage;
        [SerializeField] private float _randomFluctuationSpeed = 0.5f;
        [SerializeField] private float _fluctuationAmount = 0.005f;
        [SerializeField] private float _minHeat = 0f;
        [SerializeField] private float _maxHeat = 1f;
        [SerializeField] private float _closeThreshold = 0.05f;

        private float _currentHeat = 0.2f;
        private IEnumerator _coroutine;

        public event Action OnMinTemperatureReached;
        public event Action OnMaxTemperatureReached;
        
        public float CurrentHeat => _currentHeat;

        private void Update()
        {
            UpdatePotAlpha();
        }

        public void StarApplyingRandomFluctuations()
        {
            StopApplyingRandomFluctuations();

            _coroutine = ApplyRandomFluctuations();
            StartCoroutine(_coroutine);
        }

        public void StopApplyingRandomFluctuations()
        {
            if (_coroutine == null) return;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        public void ResetHeat()
        {
            _currentHeat = 0.2f;
        }

        public void IncreaseHeat(float amount)
        {
            _currentHeat = Mathf.Clamp(_currentHeat + amount, _minHeat, _maxHeat);
            UpdateHeatMeter();
        }

        public void DecreaseHeat(float amount)
        {
            _currentHeat = Mathf.Clamp(_currentHeat - amount, _minHeat, _maxHeat);
            UpdateHeatMeter();
        }

        private IEnumerator ApplyRandomFluctuations()
        {
            while (true)
            {
                _currentHeat = Mathf.Clamp(
                    _currentHeat + Random.Range(-_fluctuationAmount, _fluctuationAmount) * _randomFluctuationSpeed *
                    Time.deltaTime,
                    _minHeat,
                    _maxHeat
                );
                
                if (_currentHeat <= _minHeat + _closeThreshold)
                {
                    OnMinTemperatureReached?.Invoke();
                }

                if (_currentHeat >= _maxHeat - _closeThreshold)
                {
                    OnMaxTemperatureReached?.Invoke();
                }
                
                UpdateHeatMeter();

                yield return new WaitForSeconds(1f);
            }
        }

        private void UpdateHeatMeter()
        {
            _heatMeter.fillAmount = _currentHeat;
        }
        
        private void UpdatePotAlpha()
        {
            if (_potImage != null)
            {
                Color potColor = _potImage.color;
                potColor.a = Mathf.Lerp(0.1f, 1f, _currentHeat);
                _potImage.color = potColor;
            }
        }
    }
}