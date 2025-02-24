using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Onboarding _onboarding;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _onboarding.Shown += _screenVisabilityHandler.EnableScreen;
        }

        private void OnDisable()
        {
            _onboarding.Shown -= _screenVisabilityHandler.EnableScreen;
        }

        public void OpenCuttingScene()
        {
            SceneManager.LoadScene("CuttingIngredientsScene");
        }

        public void OpenStoveControlScene()
        {
            SceneManager.LoadScene("StoveControlScene");
        }

        public void OpenIngredientGatheringScene()
        {
            SceneManager.LoadScene("IngredientGatheringScene");
        }

        public void OpenRecipeStudyScene()
        {
            SceneManager.LoadScene("RecipeStudyScene");
        }
    }
}