using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gathering
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private EndGameScreen _failScreen;
        [SerializeField] private EndGameScreen _winScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Player _player;
        [SerializeField] private FruitSpawner _spawner;
        [SerializeField] private LifeHolder _lifeHolder;
        [SerializeField] private Button _pauseButton;

        private int _score;
        private int _lives;
        private int _difficulty = 10;

        private void OnEnable()
        {
            _pauseButton.onClick.AddListener(PauseGame);
            
            _player.BadFruitCatched += ProcessBadFruitCatched;
            _player.GoodFruitCatched += ProcessGoodFruitCatched;

            _failScreen.RestartGame += ProcessGameRestart;
            _failScreen.ExitGame += ExitGame;

            _winScreen.RestartGame += StartNewGame;
            _winScreen.ExitGame += ExitGame;

            _pauseScreen.ContinueGame += ContinueGame;
            _pauseScreen.ExitGame += ExitGame;
            _pauseScreen.RestartGame += ProcessGameRestart;
        }

        private void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(PauseGame);
            
            _player.BadFruitCatched -= ProcessBadFruitCatched;
            _player.GoodFruitCatched -= ProcessGoodFruitCatched;

            _failScreen.RestartGame -= ProcessGameRestart;
            _failScreen.ExitGame -= ExitGame;

            _winScreen.RestartGame -= StartNewGame;
            _winScreen.ExitGame -= ExitGame;

            _pauseScreen.ContinueGame -= ContinueGame;
            _pauseScreen.ExitGame -= ExitGame;
            _pauseScreen.RestartGame -= ProcessGameRestart;
        }

        private void Start()
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            ResetAllValues();
            _winScreen.DisableScreen();
            _failScreen.DisableScreen();
            _pauseScreen.DisableScreen();
            _spawner.StartSpawn();
        }

        private void PauseGame()
        {
            _pauseScreen.EnableScreen();
            _spawner.StopSpawn();
            _spawner.ReturnAllObjectsToPool();
        }

        private void ContinueGame()
        {
            _spawner.StartSpawn();
        }

        private void ExitGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        private void ProcessGameRestart()
        {
            ResetAllValues();
            StartNewGame();
        }

        private void ProcessGoodFruitCatched(Fruit fruit)
        {
            if (fruit is BadFruit)
            {
               ProcessBadFruitCatched(fruit);
                return;
            }

            _score++;
            _spawner.ReturnToPool(fruit);
            _scoreText.text = _score.ToString();

            if (_score >= _difficulty)
            {
                ProcessGameWin();
            }
        }

        private void ProcessGameWin()
        {
            _spawner.StopSpawn();
            _spawner.ReturnAllObjectsToPool();
            _winScreen.EnableScreen(_score.ToString());
            _difficulty *= 2;
            _spawner.IncreaseSpeed();
        }

        private void ProcessBadFruitCatched(Fruit fruit)
        {
            _lives--;
            _spawner.ReturnToPool(fruit);
            _lifeHolder.EmptyOneLife();

            if (_lives <= 0)
            {
                ProcessGameLost();
            }
        }

        private void ProcessGameLost()
        {
            _spawner.StopSpawn();
            _spawner.ReturnAllObjectsToPool();
            _failScreen.EnableScreen(_score.ToString());
        }

        private void ResetAllValues()
        {
            _lives = 3;
            _score = 0;

            _spawner.StopSpawn();
            _spawner.ReturnAllObjectsToPool();
            _lifeHolder.ResetAllLives();
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            _scoreText.text = _score.ToString();
        }
    }
}