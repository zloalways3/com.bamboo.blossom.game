using UnityEngine;

public class GameExitController : MonoBehaviour
{ 
    public void HandleGameExit()
    {
#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}