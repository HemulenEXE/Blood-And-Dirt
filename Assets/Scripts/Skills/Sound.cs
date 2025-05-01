using UnityEngine;
namespace SkillLogic
{

    public class Sound : Skill
    {
        private float _distance = 5f;
        private GameObject _light;

        public Sound()
        {
            Name = "Sound";
            IsUnlocked = false;
            _light = Resources.Load<GameObject>("Prefabs/Lights/TargetLight");
            Type = SkillType.Added;
        }
        public override void Execute(GameObject point)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.transform.position, _distance);

            foreach (var x in colliders)
            {
                if (x.gameObject.CompareTag("Enemy") || x.gameObject.CompareTag("EnemyBelievers") || x.gameObject.CompareTag("EnemyFalcons")) SpawnLight(x.transform.position);
            }
        }

        private void SpawnLight(Vector3 position)
        {
            Debug.Log("SpawnLight!");
            GameObject lightObject = GameObject.Instantiate(_light, position, Quaternion.identity);
            GameObject.Destroy(lightObject, 1f);
        }

    }
}

