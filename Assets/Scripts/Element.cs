using UnityEngine;

public class Element : MonoBehaviour
{
    private bool _isClick = false;
    public bool isBomb; // Это бомба или обычный элемент?
    public GameObject explosionPrefab; // Префаб взрыва
    public string destroyAnimationName = "DestroyAnimation"; // Имя анимации для уничтожения элемента

    private ElementSpawner spawner;
    private Animation anim;

    void Start()
    {
        spawner = FindObjectOfType<ElementSpawner>(); // Ищем объект спавнера на сцене
        anim = GetComponent<Animation>(); // Получаем компонент Animation
    }

    // Метод, который вызывается при клике на элемент
    void OnMouseDown()
    {
        if (!_isClick)
        {
            spawner.OnElementClick(isBomb, transform.position); // Передаем позицию элемента
            PlayDestroyAnimation(); // Запускаем анимацию уничтожения
            _isClick = true;
        }
    }

    // Метод для воспроизведения анимации и создания взрыва
    void PlayDestroyAnimation()
    {
        // Запускаем анимацию уничтожения объекта
        anim.Play(destroyAnimationName);

        // Создаем префаб взрыва в позиции элемента
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
        // Запускаем анимацию взрыва
        Animation explosionAnim = explosion.GetComponent<Animation>();
        if (explosionAnim != null)
        {
            explosionAnim.Play(); // Проигрываем анимацию взрыва
        }

        // Удаляем объект и взрыв через время анимации
        float destroyTime = anim.GetClip(destroyAnimationName).length;
        Destroy(explosion, destroyTime); // Удаляем взрыв через время анимации
        Destroy(gameObject, destroyTime); // Удаляем сам элемент
    }
}