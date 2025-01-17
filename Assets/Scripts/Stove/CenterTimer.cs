using System;
using TMPro;
using UnityEngine;

namespace Stove
{
    public class CenterTimer : MonoBehaviour
    {
        [SerializeField] private float _centerRange = 0.05f;
        [SerializeField] private float _centerHoldTime = 10f;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _startText;
        [SerializeField] private GameObject _timerUI;

        private float _centerTimer = 0f;
        private bool _isInCenter = false;

        public event Action GameWon;

        public void UpdateCenterTimer(float currentHeat)
        {
            _isInCenter = currentHeat >= 0.5f - _centerRange && currentHeat <= 0.5f + _centerRange;

            if (_isInCenter)
            {
                if (!_timerUI.activeSelf)
                    _timerUI.SetActive(true);
                
                _startText.gameObject.SetActive(false);
                _centerTimer += Time.deltaTime;
                _timerText.text = (_centerHoldTime - _centerTimer).ToString("F1");

                if (_centerTimer >= _centerHoldTime)
                {
                    GameWon?.Invoke();
                }
            }
            else
            {
                _timerUI.SetActive(false);
                _startText.gameObject.SetActive(true);
                _centerTimer = 0f;
            }
        }
    }
}
