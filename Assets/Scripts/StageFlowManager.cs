using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageFlowManager : MonoBehaviour
{
    public static StageFlowManager singletonInstance;   
        
    [FormerlySerializedAs("_levelButtons")] [SerializeField] private Button[] _levelOptionButtons;
    private int _stagesTotal = 24;

    void Start()
    {
        if (singletonInstance == null)
        {
            singletonInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        SetupGameStages();
        RefreshStageButtons();
    }

    private void SetupGameStages()
    {
        for (int iterationCounter = 0; iterationCounter < _stagesTotal; iterationCounter++)
        {
            if (!PlayerPrefs.HasKey(UniversalConstants.LEVEL_GAME_PREFS + iterationCounter))
            {
                PlayerPrefs.SetInt(UniversalConstants.LEVEL_GAME_PREFS + iterationCounter, iterationCounter == 0 ? 1 : 0);
            }
        }
        PlayerPrefs.Save();
    }

    public void AchieveStageVictory(int levelIndex)
    {
        PlayerPrefs.SetInt(UniversalConstants.LEVEL_GAME_PREFS + levelIndex, 1);
        PlayerPrefs.Save();
    }

    private void RefreshStageButtons()
    {
        for (int loopIndex = 0; loopIndex < _stagesTotal; loopIndex++)
        {
            if (loopIndex == 0 || PlayerPrefs.GetInt(UniversalConstants.LEVEL_GAME_PREFS + (loopIndex), 0) == 1)
            {
                _levelOptionButtons[loopIndex].interactable = true;
                _levelOptionButtons[loopIndex].image.color = Color.white;
            }
            else
            {
                _levelOptionButtons[loopIndex].interactable = false;
                _levelOptionButtons[loopIndex].image.color = new Color(1, 1, 1, 0.78f);
            }
        }
    }

    public void StageLoader(int levelIndex)
    {
        PlayerPrefs.SetInt(UniversalConstants.ACTIVE_STAGEID, levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(UniversalConstants.PrimaryGameScene);
    }
}