using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
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
