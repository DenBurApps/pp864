using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IngredientsCut
{
    public class GameController : MonoBehaviour
    {
        private const float InitTimerValue = 40;

        [SerializeField] private Button _pauseButton;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Knife _knife;
        [SerializeField] private IngredientsSpawner _ingredientsSpawner;
        [SerializeField] private IngredientToCut[] _ingredientsToCuts;
        [SerializeField] private EndGameScreen _loseScreen;
        [SerializeField] private EndGameScreen _winScreen;
        [SerializeField] private PauseScreen _pauseScreen;

        private int _spawnCount = 4;
        private int _cutIngredients;
        private float _currentTimer;
        private Ingredient _currentIngredient;

        private IEnumerator _timerCoroutine;

        private void OnEnable()
        {
            foreach (var ing in _ingredientsToCuts)
            {
                ing.Cut += PieceCut;
                ing.FullyCut += IngredientCut;
            }

            _ingredientsSpawner.IngredientComplete += () => StartCoroutine(AssignNewIngredientToCut());
            _ingredientsSpawner.Spawned += () => StartCoroutine(AssignNewIngredientToCut());

            _pauseButton.onClick.AddListener(PauseGame);
            _pauseScreen.ExitGame += ExitGame;
            _pauseScreen.ContinueGame += ContinueGame;
            _pauseScreen.RestartGame += StartNewGame;

            _loseScreen.ExitGame += ExitGame;
            _loseScreen.RestartGame += StartNewGame;

            _winScreen.ExitGame += ExitGame;
            _winScreen.RestartGame += StartNewGame;
        }

        private void OnDisable()
        {
            foreach (var ing in _ingredientsToCuts)
            {
                ing.Cut -= PieceCut;
                ing.FullyCut -= IngredientCut;
            }

            _ingredientsSpawner.IngredientComplete -= () => StartCoroutine(AssignNewIngredientToCut());
            _ingredientsSpawner.Spawned -= () => StartCoroutine(AssignNewIngredientToCut());

            _pauseButton.onClick.RemoveListener(PauseGame);
            _pauseScreen.ExitGame -= ExitGame;
            _pauseScreen.ContinueGame -= ContinueGame;
            _pauseScreen.RestartGame -= StartNewGame;

            _loseScreen.ExitGame -= ExitGame;
            _loseScreen.RestartGame -= StartNewGame;

            _winScreen.ExitGame -= ExitGame;
            _winScreen.RestartGame -= StartNewGame;
        }

        private void Start()
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            ResetValues();
            _ingredientsSpawner.ActivateIngredients(_spawnCount);
            StartTimerCoroutine();
            _knife.EnableInputDetection();
        }

        private void ResetValues()
        {
            foreach (var ing in _ingredientsToCuts)
            {
                ing.gameObject.SetActive(false);
            }
            
            _pauseScreen.DisableScreen();
            _winScreen.DisableScreen();
            _loseScreen.DisableScreen();
            _ingredientsSpawner.ReturnAllObjectsToPool();
            _currentTimer = InitTimerValue;
            _cutIngredients = 0;
        }

        private void PauseGame()
        {
            StopTimerCoroutine();
            _knife.StopInputDetection();

            _pauseScreen.EnableScreen();
        }

        private void ContinueGame()
        {
            StartTimerCoroutine();
            _knife.EnableInputDetection();
        }

        private void UpdateTimerText()
        {
            _timerText.text = _currentTimer.ToString("F2");
        }

        private void StartTimerCoroutine()
        {
            StopTimerCoroutine();

            _timerCoroutine = StartTimer();
            StartCoroutine(_timerCoroutine);
        }

        private void StopTimerCoroutine()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }

        private IEnumerator StartTimer()
        {
            while (_currentTimer > 0)
            {
                yield return null;
                _currentTimer -= Time.deltaTime;
                UpdateTimerText();

                if (AllIngredientsCut())
                {
                    ProcessWin();
                    yield break;
                }
            }

            ProcessLoss();
        }

        private bool AllIngredientsCut()
        {
            return _cutIngredients == _spawnCount;
        }

        private IEnumerator AssignNewIngredientToCut()
        {
            _currentIngredient = _ingredientsSpawner.GetFirstIngredient();

            yield return new WaitForSeconds(1f);

            var ingredient = _ingredientsToCuts.FirstOrDefault(ing => ing.Type == _currentIngredient.Type);

            if (ingredient != null)
            {
                ingredient.gameObject.SetActive(true);
            }
        }

        private void PieceCut()
        {
            _currentIngredient.IncreaseProgress();
        }

        private void IngredientCut()
        {
            _cutIngredients++;
        }

        private void ProcessWin()
        {
            _spawnCount++;
            _ingredientsSpawner.ReturnAllObjectsToPool();
            _knife.StopInputDetection();
            _winScreen.EnableScreen(_currentTimer);
        }

        private void ProcessLoss()
        {
            _knife.StopInputDetection();
            _ingredientsSpawner.ReturnAllObjectsToPool();
            _loseScreen.EnableScreen(_currentTimer);
        }

        private void ExitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}