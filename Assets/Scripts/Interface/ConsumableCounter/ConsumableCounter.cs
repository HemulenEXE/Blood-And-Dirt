using System;
using TMPro;
using UnityEngine;

namespace InventoryLogic
{
    /// <summary>
    /// Хранит кол-во каждого расходника, а также управляет его отображением на канвасе. 
    /// Навешивается на элемент ConsumablesUI в канвасе
    /// </summary>
    public class ConsumableCounter : MonoBehaviour
    {
        //Поля.
        
        /// <summary>
        /// Единственный экземпляр счётчика расходников.
        /// </summary>
        private static ConsumableCounter _instance;
        /// <summary>
        /// Иконка дымовой гранаты.
        /// </summary>
        private Transform _smokeGrenadeIcon;
        /// <summary>
        /// Иконка простой гранаты
        /// </summary>
        private Transform _simpleGrenadeIcon;
        /// <summary>
        /// Иконка аптечки.
        /// </summary>
        private Transform _firstAidKitIcon;
        /// <summary>
        /// Иконка бинта.
        /// </summary>
        private Transform _bandageIcon;
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
        /// Максимальное число дымовых гранат.
        /// </summary>
        [SerializeField] private static int _maxCountSmokeGrenade = 5;
        /// <summary>
        /// Максимальное число простых гранат.
        /// </summary>
        [SerializeField] private static int _maxCountSimpleGrenade = 5;
        /// <summary>
        /// Максимальное число аптечек.
        /// </summary>
        [SerializeField] private static int _maxCountFirstAidKit = 5;
        /// <summary>
        /// Максимальное число бинтов.
        /// </summary>
        [SerializeField] private static int _maxCountBandage = 5;

        //Свойства.

        /// <summary>
        /// Возвращает единственный экземпляр счётчика расходинков.
        /// </summary>
        public static ConsumableCounter GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<ConsumableCounter>();

                    if (_instance == null)
                    {
                        var temp = GameObject.Find("Inventory&ConsumableCounter")?.AddComponent<ConsumableCounter>();
                        if (temp == null) throw new ArgumentNullException("Inventory: Scene doesn't have the canvas \"Inventory&ConsumableCounter\"");
                        _instance = temp;
                    }
                }
                return _instance;
            }
        }
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
                if (value < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: value < 0");
                if (value > _maxCountSmokeGrenade) throw new ArgumentOutOfRangeException("ConsumableCounter: value > _maxCountSmokeGrenade");
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
                if (value < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: value < 0");
                if (value > _maxCountSimpleGrenade) throw new ArgumentOutOfRangeException("ConsumableCounter: value > _maxCountSimpleGrenade");
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
                if (value < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: value < 0");
                if (value > _maxCountFirstAidKit) throw new ArgumentOutOfRangeException("ConsumableCounter: value > _maxCountFirstAidKit");
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
                if (value < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: value < 0");
                if (value > _maxCountBandage) throw new ArgumentOutOfRangeException("ConsumableCounter: value > _maxCountBandage");
                _bandageCount = value;
            }
        }

        //Встроенные методы.

        /// <summary>
        /// Проверка и настройка полей.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void Awake()
        {
            _smokeGrenadeIcon = this.transform.Find("UIPanel")?.GetChild(0);
            _simpleGrenadeIcon = this.transform.Find("UIPanel")?.GetChild(1);
            _firstAidKitIcon = this.transform.Find("UIPanel")?.GetChild(3);
            _bandageIcon = this.transform.Find("UIPanel")?.GetChild(4);

            if (_smokeGrenadeIcon == null) throw new ArgumentNullException("ConsumableCounter: _smokeGrenadeIcon is null");
            if (_simpleGrenadeIcon == null) throw new ArgumentNullException("ConsumableCounter: _simpleGrenadeIcon is null");
            if (_firstAidKitIcon == null) throw new ArgumentNullException("ConsumableCounter: _firstAidKitIcon is null");
            if (_bandageIcon == null) throw new ArgumentNullException("ConsumableCounter: _bandageIcon is null");

            if (_smokeGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("ConsumableCounter: _smokeGrenadeIcon doesn't have TextMeshProUGUI");
            if (_simpleGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("ConsumableCounter: _simpleGrenadeIcon doesn't have TextMeshProUGUI");
            if (_firstAidKitIcon.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("ConsumableCounter: _firstAidKitIcon doesn't have TextMeshProUGUI");
            if (_bandageIcon.GetComponentInChildren<TextMeshProUGUI>() == null) throw new ArgumentNullException("ConsumableCounter: _bandageIcon doesn't have TextMeshProUGUI");

            if (_maxCountSmokeGrenade < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: _maxCountSmokeGrenade < 0");
            if (_maxCountSimpleGrenade < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: _maxCountSimpleGrenade < 0");
            if (_maxCountFirstAidKit < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: _maxCountFirstAidKit < 0");
            if (_maxCountBandage < 0) throw new ArgumentOutOfRangeException("ConsumableCounter: _maxCountBandage < 0");
        }
        /// <summary>
        /// Обновление текстовой информации на канвасе о количестве расходников.
        /// </summary>
        private void FixedUpdate()
        {
            _smokeGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>().text = SmokeGrenadeCount.ToString();
            _smokeGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>().text = SimpleGrenadeCount.ToString();
            _smokeGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>().text = FirstAidKitCount.ToString();
            _smokeGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>().text = BandageCount.ToString();
        }

        //Вспомогательные методы.

        /// <summary>
        /// Аннулирование всех расходников.
        /// </summary>
        public static void Clear()
        {
            SmokeGrenadeCount = 0;
            SimpleGrenadeCount = 0;
            FirstAidKitCount = 0;
            BandageCount = 0;
        }
    }

}