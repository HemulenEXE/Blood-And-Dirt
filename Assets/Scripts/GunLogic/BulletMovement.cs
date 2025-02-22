using UnityEngine;

namespace GunLogic
{
    public class BulletMovement : MonoBehaviour
    {
        private float _speed;
        private float _lifeTime = 5f; // ����� ����� ���� (� ��������)

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        void Start()
        {
            // ���������� ���� ����� lifeTime ������
            Destroy(gameObject, _lifeTime);
        }

        void Update()
        {
            // ������� ���� ������
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
    }
}
