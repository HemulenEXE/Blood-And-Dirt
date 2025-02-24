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

        private Transform _smokeGrenadeIcon;
        private Transform _simpleGrenadeIcon;
        private Transform _firstAidKitIcon;
        private Transform _bandageIcon;

        public static int _smokeGrenadeCount = 0;
        public static int _simpleGrenadeCount = 0;
        public static int _firstAidKitCount = 0;
        public static int _bandageCount = 0;

        public const int _maxCountSmokeGrenade = 5;
        public const int _maxCountSimpleGrenade = 5;
        public const int _maxCountFirstAidKit = 5;
        public const int _maxCountBandage = 5;

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
            _smokeGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>().text = _smokeGrenadeCount.ToString();
            _simpleGrenadeIcon.GetComponentInChildren<TextMeshProUGUI>().text = _simpleGrenadeCount.ToString();
            _firstAidKitIcon.GetComponentInChildren<TextMeshProUGUI>().text = _firstAidKitCount.ToString();
            _bandageIcon.GetComponentInChildren<TextMeshProUGUI>().text = _bandageCount.ToString();
        }

        //Вспомогательные методы.

        /// <summary>
        /// Аннулирование всех расходников.
        /// </summary>
        public static void Clear()
        {
            _smokeGrenadeCount = 0;
            _simpleGrenadeCount = 0;
            _firstAidKitCount = 0;
            _bandageCount = 0;
        }
    }

}