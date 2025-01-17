using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RecipeStudy
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private LifeHolder _lifeHolder;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _topText;
        [SerializeField] private RecipeElementHolder _recipeElementHolder;
        [SerializeField] private EndGameScreen _loseScreen;
        [SerializeField] private EndGameScreen _winScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private StartScreen _startScreen;

        private ScreenVisabilityHandler _screenVisabilityHandler;
        
        private readonly int _startDifficulty = 3;
        private readonly int _maxDifficulty = 9;
        private int _difficulty;
        private int _lives;
        private int _score;

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _startScreen.StartClicked += StartNewGame;

            _loseScreen.ExitGame += ExitGame;
            _loseScreen.RestartGame += StartNewGame;

            _winScreen.ExitGame += ExitGame;
            _winScreen.RestartGame += StartNewGame;
            
            _pauseButton.onClick.AddListener(PauseGame);
            _pauseScreen.RestartGame += StartNewGame;
            _pauseScreen.ExitGame += ExitGame;
            _pauseScreen.ContinueGame += ContinueGame;

            _recipeElementHolder.AllElementsShown += EnableUI;
            _recipeElementHolder.ElementCorrectlyChosen += ElementCorrectlyChosen;
            _recipeElementHolder.ElementIncorrectlyChosen += ElementIncorrectlyChosen;
            _recipeElementHolder.AllElementsCorrectlyChosen += GameWin;
        }

        private void OnDisable()
        {
            _startScreen.StartClicked -= StartNewGame;

            _loseScreen.ExitGame -= ExitGame;
            _loseScreen.RestartGame -= StartNewGame;

            _winScreen.ExitGame -= ExitGame;
            _winScreen.RestartGame -= StartNewGame;
            
            _pauseButton.onClick.RemoveListener(PauseGame);
            _pauseScreen.RestartGame -= StartNewGame;
            _pauseScreen.ExitGame -= ExitGame;
            _pauseScreen.ContinueGame -= ContinueGame;
            
            _recipeElementHolder.AllElementsShown -= EnableUI;
            _recipeElementHolder.ElementCorrectlyChosen -= ElementCorrectlyChosen;
            _recipeElementHolder.ElementIncorrectlyChosen -= ElementIncorrectlyChosen;
            _recipeElementHolder.AllElementsCorrectlyChosen -= GameWin;
        }

        private void Start()
        {
            _difficulty = _startDifficulty;
            DisableScreens();
            _recipeElementHolder.DisableAllElements();
            _screenVisabilityHandler.DisableScreen();
        }

        private void StartNewGame()
        {
            _screenVisabilityHandler.EnableScreen();
            ResetAllValues();
            DisableScreens();
            DisableUIElements();
            _recipeElementHolder.DisableAllElements();
            _recipeElementHolder.SetSequenceCount(_difficulty);
            _recipeElementHolder.StartSequence();
        }

        private void DisableScreens()
        {
            _winScreen.DisableScreen();
            _loseScreen.DisableScreen();
            _pauseScreen.DisableScreen();
        }

        private void DisableUIElements()
        {
            _lifeHolder.gameObject.SetActive(false);
            _scoreText.gameObject.SetActive(false);
            _topText.gameObject.SetActive(false);
        }

        private void EnableUI()
        {
            _recipeElementHolder.EnableAllElements();
            _lifeHolder.gameObject.SetActive(true);
            _scoreText.gameObject.SetActive(true);
            _topText.gameObject.SetActive(true);
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            _scoreText.text = _score.ToString();
        }

        private void ResetAllValues()
        {
            _lives = 3;
            _score = 0;
            _lifeHolder.ResetAllLives();
        }

        private void ElementCorrectlyChosen()
        {
            _score++;
            UpdateScoreText();
        }

        private void ElementIncorrectlyChosen()
        {
            _lives--;
            _lifeHolder.EmptyOneLife();

            if (_lives <= 0)
            {
                GameLost();
            }
        }

        private void GameLost()
        {
            _loseScreen.EnableScreen(_score.ToString());
        }

        private void GameWin()
        {
            _winScreen.EnableScreen(_score.ToString());
            
            if (_difficulty < _maxDifficulty)
            {
                _difficulty++;
            }
        }

        private void PauseGame()
        {
            _recipeElementHolder.PauseSequence();
            _pauseScreen.EnableScreen();
        }

        private void ContinueGame()
        {
            _recipeElementHolder.ResumeSequence();
        }

        private void ExitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
