using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayFlowController : MonoBehaviour
{
    [FormerlySerializedAs("_levelText")] [SerializeField] private TextMeshProUGUI[] stageDisplayText;
        
    private int activeStageID;
        
    private void Start()
    {
        activeStageID = PlayerPrefs.GetInt(UniversalConstants.ACTIVE_STAGEID, 0);
        for (int indexCounterTipe = 0; indexCounterTipe < stageDisplayText.Length; indexCounterTipe++)
        {
            stageDisplayText[indexCounterTipe].text = "Level " + activeStageID;
        }
    }

    public void CompleteVictory()
    {
        PlayerPrefs.SetInt(UniversalConstants.ACTIVE_STAGEID, activeStageID+1);
        PlayerPrefs.Save();
        StageFlowManager stageManager = FindObjectOfType<StageFlowManager>();
        stageManager.AchieveStageVictory(activeStageID);
    }
}