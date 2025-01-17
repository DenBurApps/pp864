using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RecipeStudy
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _startButton;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        public event Action StartClicked;

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _homeButton.onClick.AddListener(ExitGame);
            _startButton.onClick.AddListener(OnStartClicked);
        }

        private void OnDisable()
        {
            _homeButton.onClick.RemoveListener(ExitGame);
            _startButton.onClick.RemoveListener(OnStartClicked);
        }

        private void Start()
        {
            _screenVisabilityHandler.EnableScreen();
        }

        private void ExitGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        private void OnStartClicked()
        {
            StartClicked?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }
}