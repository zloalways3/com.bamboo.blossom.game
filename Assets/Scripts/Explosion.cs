using UnityEngine;

public class Explosion : MonoBehaviour
{
    public string explosionAnimationName = "ExplosionAnimation"; // Имя анимации взрыва

    void Start()
    {
        Animation anim = GetComponent<Animation>();
        if (anim != null)
        {
            anim.Play(explosionAnimationName); // Проигрываем анимацию взрыва
            Destroy(gameObject, anim.GetClip(explosionAnimationName).length); // Удаляем объект после окончания анимации
        }
    }
}