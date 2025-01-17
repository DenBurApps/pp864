using System.Collections;
using UnityEngine;

namespace Stove
{
    public class Stove : MonoBehaviour
    {
        [SerializeField] private HeatMeter _heatMeter;
        [SerializeField] private CenterTimer _centerTimer;
        [SerializeField] private float _heatIncreaseRate = 0.01f;
        [SerializeField] private float _heatDecreaseRate = 0.01f;

        private IEnumerator _centerTimerCoroutine;

        public void StartCenterTimerCoroutine()
        {
            StopCenterTimerCoroutine();

            _centerTimerCoroutine = CenterTimerCoroutine();
            StartCoroutine(_centerTimerCoroutine);
        }

        public void StopCenterTimerCoroutine()
        {
            if (_centerTimerCoroutine == null)
                return;

            StopCoroutine(_centerTimerCoroutine);
            _centerTimerCoroutine = null;
        }

        private IEnumerator CenterTimerCoroutine()
        {
            while (true)
            {
                _centerTimer.UpdateCenterTimer(_heatMeter.CurrentHeat);
                
                yield return null;
            }
        }

        public void OnIncreaseHeatButton()
        {
            _heatMeter.IncreaseHeat(_heatIncreaseRate);
        }

        public void OnDecreaseHeatButton()
        {
            _heatMeter.DecreaseHeat(_heatDecreaseRate);
        }
    }
}