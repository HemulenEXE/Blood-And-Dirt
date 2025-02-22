using System;
using TMPro;
using UnityEngine;

namespace InventoryLogic
{
    /// <summary>
    /// Хранит кол-во каждого расходника, а также управляет его отображением на канвасе. 
    /// Навешивается на элемент ConsumablesUI в канвасе
    /// </summary>
    public class ConsumablesCounter : MonoBehaviour
    {
        /// <summary>
        /// Иконка дымовой гранаты.
        /// </summary>
        private Transform _smokeGrenade;
        /// <summary>
        /// Иконка простой гранаты
        /// </summary>
        private Transform _simpleGrenade;
        /// <summary>
        /// Иконка аптечки.
        /// </summary>
        private Transform _firstAidKit;
        /// <summary>
        /// Иконка бинта.
        /// </summary>
        private Transform _bandage;
        /// <summary>
        /// Количество дымовых гранат.
        /// </summary>
        private static int _smokeGrenadeCount = 0;
        /// <summary>
        /// Количество простых гранат.
        /// </summary>
        private static int _simpleGrenadeCount = 0;
        /// <summary>
        /// Количество аптечек.
        /// </summary>
        private static int _firstAidKitCount = 0;
        /// <summary>
        /// Количество бинтов.
        /// </summary>
        private static int _bandageCount = 0;
        /// <summary>
        /// Возвращает и изменяет количество дымовых гранат.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int SmokeGrenadeCount
        {
            get
            {
                return _smokeGrenadeCount;
            }
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException("Invalid value!");
                _smokeGrenadeCount = value;
            }
        }
        /// <summary>
        /// Возвращает и изменяет количество простых гранат.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int SimpleGrenadeCount
        {
            get
            {
                return _simpleGrenadeCount;
            }
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException("Invalid value!");
                _simpleGrenadeCount = value;
            }
        }
        /// <summary>
        /// Возвращает и изменяет количество аптечек.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int FirstAidKitCount
        {
            get
            {
                return _firstAidKitCount;
            }
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException("Invalid value!");
                _firstAidKitCount = value;
            }
        }
        /// <summary>
        /// Возвращает и изменяет количество бинтов.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int BandageCount
        {
            get
            {
                return _bandageCount;
            }
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException("Invalid value!");
                _bandageCount = value;
            }
        }
        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void Awake()
        {
            _smokeGrenade = this.transform.GetChild(0);
            _simpleGrenade = this.transform.GetChild(1);
            _firstAidKit = this.transform.GetChild(3);
            _bandage = this.transform.GetChild(4);

            if (_smokeGrenade == null) throw new ArgumentNullException("Consumables: _smokeGrenade is null");
            if (_simpleGrenade == null) throw new ArgumentNullException("Consumables: _simpleGrenade is null");
            if (_firstAidKit == null) throw new ArgumentNullException("Consumables: _firstAidKit is null");
            if (_bandage == null) throw new ArgumentNullException("Consumables: _bandage is null");
            if (_smokeGrenade.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("Consumables: _smokeGrenade hasn't TextMeshProUGUI");
            if (_simpleGrenade.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("Consumables: _simpleGrenade hasn't TextMeshProUGUI");
            if (_firstAidKit.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("Consumables: _firstAidKit hasn't TextMeshProUGUI");
            if (_bandage.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("Consumables: _bandage hasn't TextMeshProUGUI");
        }
        /// <summary>
        /// Обновление текстовой информации на канвасе о количестве расходников.
        /// </summary>
        private void FixedUpdate()
        {
            _smokeGrenade.GetComponentInChildren<TextMeshProUGUI>().text = SmokeGrenadeCount.ToString();
            _simpleGrenade.GetComponentInChildren<TextMeshProUGUI>().text = SimpleGrenadeCount.ToString();
            _firstAidKit.GetComponentInChildren<TextMeshProUGUI>().text = FirstAidKitCount.ToString();
            _bandage.GetComponentInChildren<TextMeshProUGUI>().text = BandageCount.ToString();
        }
    }

}