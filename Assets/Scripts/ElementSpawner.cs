using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ElementSpawner : MonoBehaviour
{
    public GameObject[] elements; // Массив с префабами обычных элементов
    public GameObject bombPrefab;  // Префаб бомбы
    public GameObject scorePopupPrefab; // Префаб всплывающего текста для очков
    public GameObject spawnTarget; // Объект, на котором будут спавниться элементы
    [FormerlySerializedAs("spawnInterval")] public float objectSpawnDelay = 1.5f; // Интервал появления новых элементов
    [FormerlySerializedAs("scoreText")] public TextMeshProUGUI pointsDisplayText; // Текст для отображения очков
    [FormerlySerializedAs("timerText")] public TextMeshProUGUI countdownDisplayText; // Текст для отображения таймера
    public GameObject winMenu; // Меню выигрыша
    public GameObject loseMenu; // Меню проигрыша
    public float horizontalMargin = 1.0f; // Горизонтальный отступ от границ экрана
    public float topMargin = 1.0f; // Отступ сверху от границ экрана
    public float bottomMargin = 1.0f; // Отступ снизу от границ экрана
    public GameObject _gameSceneObject;
    public TextMeshProUGUI[] _textScoreManagerMenu;
    
    private int playerScore = 0;
    private float remainingTime = 85f; // Время в секундах (1:25 = 85 секунд)
    private bool isGameOver = false;
    private int targetScore = 600; // Цель набрать 600 очков
    private Vector2 screenBounds; // Границы экрана для случайного спавна
    private int _currentLevel;

    void Start()
    {
        // Определяем границы экрана
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(SpawnElements());
        RefreshScoreDisplay();
        _currentLevel = PlayerPrefs.GetInt(UniversalConstants.ACTIVE_STAGEID, 0);
        targetScore = (_currentLevel * targetScore) / 2;
        RefreshScoreDisplay();
    }

    void Update()
    {
        if (!isGameOver)
        {
            UpdateTimer();
        }
    }

    // Метод для спавна элементов с интервалом
    IEnumerator SpawnElements()
    {
        while (!isGameOver)
        {
            SpawnRandomElement();
            yield return new WaitForSeconds(objectSpawnDelay);
        }
    }

    // Метод для спавна случайного элемента с учетом границ экрана и отступов
    void SpawnRandomElement()
    {
        // Выбираем случайно между обычным элементом и бомбой
        GameObject elementToSpawn = Random.value > 0.2f ? elements[Random.Range(0, elements.Length)] : bombPrefab;

        // Случайная позиция на экране, с учётом границ и отступов
        Vector2 spawnPosition = new Vector2(
            Random.Range(-screenBounds.x + horizontalMargin, screenBounds.x - horizontalMargin), // Горизонтальный отступ
            Random.Range(-screenBounds.y + bottomMargin, screenBounds.y - topMargin)  // Отступ сверху и снизу
        );

        // Создаем объект как дочерний для spawnTarget
        GameObject newElement = Instantiate(elementToSpawn, spawnPosition, Quaternion.identity, spawnTarget.transform);

        // Уничтожаем объект через 7 секунд
        Destroy(newElement, 7.0f);
    }

    // Метод для обработки нажатия на элемент
    public void OnElementClick(bool isBomb, Vector3 position)
    {
        if (isGameOver) return; // Игнорируем клики, если игра окончена

        int playerPoints = isBomb ? -100 : 50; // Если бомба, -100 очков, иначе +50
        playerScore += playerPoints;
        ShowScorePopup(playerPoints, position); // Показываем всплывающий текст
        RefreshScoreDisplay();
        CheckForWin();
    }

    // Показ всплывающего текста очков
    void ShowScorePopup(int points, Vector3 position)
    {
        GameObject popup = Instantiate(scorePopupPrefab, position, Quaternion.identity);
        TextMeshProUGUI popupText = popup.GetComponentInChildren<TextMeshProUGUI>();
        popupText.text = (points > 0 ? "+" : "") + points.ToString();

        Destroy(popup, 0.7f); // Уничтожаем всплывающий текст через 1 секунду
    }

    // Обновление текста очков
    void RefreshScoreDisplay()
    {
        foreach (var textMeshProArray in _textScoreManagerMenu)
        {
            textMeshProArray.text = playerScore.ToString();
        }
        
        pointsDisplayText.text = $"SCORE\n {playerScore}/{targetScore}";
    }

    // Обновление таймера
    void UpdateTimer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int elapsedMinutes = Mathf.FloorToInt(remainingTime / 60);
            int elapsedSeconds = Mathf.FloorToInt(remainingTime % 60);
            countdownDisplayText.text = $"{elapsedMinutes}:{elapsedSeconds:D2} min";
        }
        else
        {
            remainingTime = 0;
            TerminateGameplay(false); // Время истекло — проигрыш
        }
    }

    // Проверка на победу
    void CheckForWin()
    {
        if (playerScore >= targetScore)
        {
            TerminateGameplay(true); // Игрок набрал нужное количество очков — победа
        }
    }

    public void TimeOn()
    {
        Time.timeScale = 1f;
    }
    public void TimeOff()
    {
        Time.timeScale = 0f;
    }

    // Завершение игры
    void TerminateGameplay(bool didWin)
    {
        isGameOver = true;
        StopAllCoroutines(); // Останавливаем спавн элементов
        _gameSceneObject.SetActive(false);

        if (didWin)
        {
            winMenu.SetActive(true); // Показываем меню победы
        }
        else
        {
            loseMenu.SetActive(true); // Показываем меню проигрыша
        }
    }
}
