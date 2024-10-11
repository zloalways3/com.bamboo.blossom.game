using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCoordinator : MonoBehaviour
{
    public void DisplayStartupScreen()
    {
        SceneManager.LoadScene(UniversalConstants.SceneLoadingMessage);
    }
    public void LoadGameplayScreen()
    {
        SceneManager.LoadScene(UniversalConstants.PrimaryGameScene);
    }
        
}