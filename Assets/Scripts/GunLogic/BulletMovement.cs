using UnityEngine;

namespace GunLogic
{
    public class BulletMovement : MonoBehaviour
    {
        private float _speed;
        private float _lifeTime = 5f; // ¬рем€ жизни пули (в секундах)

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        void Start()
        {
            // ”ничтожить пулю через lifeTime секунд
            Destroy(gameObject, _lifeTime);
        }

        void Update()
        {
            // ƒвигать пулю вперед
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
    }
}
