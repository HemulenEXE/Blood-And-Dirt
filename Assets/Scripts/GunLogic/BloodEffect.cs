using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace CameraLogic.CameraEffects
{
    /// <summary>
    /// Скрипт управления эффектом крови на экране. Вспомогательный 
    /// </summary>
    public class BloodEffect : MonoBehaviour
    {
        /// <summary>
        /// Картинка на канвасе
        /// </summary>
        private Image _image;
        /// <summary>
        /// Картинки эффекта крови
        /// </summary>
        private List<Sprite> _images = new List<Sprite>();
        /// <summary>
        /// Кол-во спрайтов 
        /// </summary>
        private int _spritesCount = 4;
        /// <summary>
        /// Текущий спрайт
        /// </summary>
        private int _currentSprite = 0;
        private void Start()
        {
            _image = GameObject.Find("BloodEffect").transform.Find("Image").GetComponent<Image>();
            for (int i = 0; i < _spritesCount; i++)
                _images.Add(Resources.Load<Sprite>("Sprites/Interface/Blood" + i));
        }
        /// <summary>
        /// Переключает канвас вперёд на заданное кол-во картинок от текущей 
        /// </summary>
        /// <param name="count"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void NextAnyBloodEffect(int count)
        {
            if (_currentSprite + count > _spritesCount)
                throw new ArgumentOutOfRangeException("Invalid count!");
            for (int i = 0; i < count; i++)
                NextBloodEffect();
            Debug.Log(_currentSprite);
        }
        /// <summary>
        /// Переключает назад на заданное кол-во картинок от текущей 
        /// </summary>
        /// <param name="count"></param>
        public void PrevAnyBloodEffect(int count)
        {
            if (_currentSprite - count < -1)
                throw new ArgumentOutOfRangeException("Invalid count!");
            for (int i = 0; i < count; i++)
                PrevBloodEffect();
            Debug.Log(_currentSprite);
        }
        /// <summary>
        /// Переключает на следующую картинку
        /// </summary>
        private void NextBloodEffect()
        {
            if (_currentSprite == 0)
                _image.gameObject.SetActive(true);

            _image.sprite = _images[_currentSprite];
            _currentSprite = _currentSprite == _spritesCount - 1 ? _currentSprite : _currentSprite + 1;

        }
        /// <summary>
        /// Переключает на предыдущую картинку
        /// </summary>
        private void PrevBloodEffect()
        {
            if (_currentSprite == 0)
                _image.gameObject.SetActive(false);
            else
            {
                _currentSprite--;
                _image.sprite = _images[_currentSprite];
            }
        }
    }
}