using UnityEngine;

namespace SkillLogic
{
    public class Hatred : Skill
    {
        private float _newRunSpeed = 20f;
        private float _oldRunSpeed;

        public Hatred()
        {
            Name = "Hatred";
            IsUnlocked = false;
            _oldRunSpeed = PlayerData.RunSpeed;
            Type = SkillType.Activated;
        }

        public override void Execute(GameObject point)
        {
            if (PlayerData.IsBleeding)
            {
                PlayerData.IsRunning = true;
                PlayerData.RunSpeed = _newRunSpeed;
                //var bodies = GameObject.FindGameObjectsWithTag("Body");
                // Уменьшение целостности трупов - можно вызвать отдельно метод
            }
            else PlayerData.RunSpeed = _oldRunSpeed;
        }
    }
}
