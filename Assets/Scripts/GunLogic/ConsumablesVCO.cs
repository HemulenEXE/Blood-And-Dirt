using TMPro;
using UnityEngine;

namespace InteractiveObjects{
    /// <summary>
    /// Класс, реализующий "расходники".<br/>
    /// Этот скрипт позволяет забирать расходники (аптечку, бинты, гранаты) со сцены и изменяет показатель их количества на канвасе.
    /// </summary>
    public class ConsumablesVCO : ClickedObject
    {
        /// <summary>
        /// Объект, показывающий текущее количество расходника у игрока.
        /// </summary>
        private GameObject _count;
        /// <summary>
        /// Настройка и проверка полей.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Transform panel = GameObject.FindGameObjectWithTag("Canvas")?.transform?.Find("ConsumablesUI");
            Transform icon = panel.Find(this.tag); //Иконка расходника на канвасе.
            _count = icon.GetChild(0).gameObject;
        }
        /// <summary>
        /// Взаимодействие с расходником.
        /// </summary>
        public override void Interact()
        {
            if (_count.GetComponent<TextMeshProUGUI>().text != "5")
            {
                Destroy(this.gameObject);
                _count.GetComponent<TextMeshProUGUI>().text = (int.Parse(_count.GetComponent<TextMeshProUGUI>().text) + 1).ToString();
            }
        }
    }
}