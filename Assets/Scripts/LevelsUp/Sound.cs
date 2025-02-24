using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace SkillLogic
{

    public class Sound : Skill
    {
        private float _distance = 5f;
        private GameObject _light;

        public Sound()
        {
            _name = "Sound";
            _isUnlocked = false;
            _previousSkills = new List<Skill>();
            _light = Resources.Load<GameObject>("Prefabs/Lights/TargetLight");

        }
        public override void Execute(GameObject point)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.transform.position, _distance);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Loop");
                    SpawnLight(collider.transform.position);
                }
            }
        }

        private void SpawnLight(Vector3 position)
        {
            Debug.Log("SpawnLight!");
            GameObject lightObject = GameObject.Instantiate(_light, position, Quaternion.identity);
            GameObject.Destroy(lightObject, 0.5f);
        }

    }
}

