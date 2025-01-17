using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Stove
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private HeatMeter _heatMeter;
        [SerializeField] private CenterTimer _centerTimer;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private EndGameScreen _loseScreen;
        [SerializeField] private EndGameScreen _winScreen;
        [SerializeField] private Stove _stove;
        [SerializeField] private Button _pauseButton;

        private void OnEnable()
        {
            _pauseButton.onClick.AddListener(PauseGame);
            
            _heatMeter.OnMinTemperatureReached += ProcessColdLoss;
            _heatMeter.OnMaxTemperatureReached += ProcessOverheatLoss;

            _centerTimer.GameWon += ProcessGameWon;

            _loseScreen.ExitGame += ExitGame;
            _winScreen.ExitGame += ExitGame;

            _loseScreen.RestartGame += StartNewGame;
            _winScreen.RestartGame += StartNewGame;

            _pauseScreen.ExitGame += ExitGame;
            _pauseScreen.RestartGame += StartNewGame;
            _pauseScreen.ContinueGame += ContinueGame;
        }

        private void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(PauseGame);
            
            _heatMeter.OnMinTemperatureReached -= ProcessColdLoss;
            _heatMeter.OnMaxTemperatureReached -= ProcessOverheatLoss;

            _centerTimer.GameWon -= ProcessGameWon;

            _loseScreen.ExitGame -= ExitGame;
            _winScreen.ExitGame -= ExitGame;

            _loseScreen.RestartGame -= StartNewGame;
            _winScreen.RestartGame -= StartNewGame;

            _pauseScreen.ExitGame -= ExitGame;
            _pauseScreen.RestartGame -= StartNewGame;
            _pauseScreen.ContinueGame -= ContinueGame;
        }

        private void Start()
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            _pauseScreen.DisableScreen();
            _loseScreen.DisableScreen();
            _winScreen.DisableScreen();
            
            _heatMeter.ResetHeat();
            _stove.StartCenterTimerCoroutine();
            _heatMeter.StarApplyingRandomFluctuations();
        }

        private void ProcessOverheatLoss()
        {
            _loseScreen.EnableScreen("DISH BURNED");
            StopGame();
        }

        private void ProcessColdLoss()
        {
            _loseScreen.EnableScreen("DISH HAS COOLED");
            StopGame();
        }

        private void ProcessGameWon()
        {
            _winScreen.EnableScreen("DISH DONE");
            StopGame();
        }

        private void PauseGame()
        {
            _pauseScreen.EnableScreen();
            
            _stove.StopCenterTimerCoroutine();
            _heatMeter.StopApplyingRandomFluctuations();
        }

        private void ContinueGame()
        {
            _stove.StartCenterTimerCoroutine();
            _heatMeter.StarApplyingRandomFluctuations();
        }
        
        private void StopGame()
        {
            _heatMeter.ResetHeat();
            _stove.StopCenterTimerCoroutine();
            _heatMeter.StopApplyingRandomFluctuations();
        }

        private void ExitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
