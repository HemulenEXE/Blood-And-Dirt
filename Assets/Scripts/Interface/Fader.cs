using System;
using UnityEngine;

namespace CameraLogic.CameraEffects
{
    /// <summary>
    /// Класс, реализующий "затемнение и осветление экрана".
    /// </summary>
    public class Fader : MonoBehaviour
    {
        /// <summary>
        /// Компонент одиночка.
        /// </summary>
        private static Fader _instance;
        /// <summary>
        /// Компонент, отвечающий за анимацию перехода.
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// Обратный вызов затемнения экрана.
        /// </summary>
        private Action _fadeInCallback;
        /// <summary>
        /// Обратный вызов осветления экрана.
        /// </summary>
        private Action _fadeOutCallback;
        /// <summary>
        /// Возвращает компонент одиночка.
        /// </summary>
        public static Fader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<Fader>("Prefabs/Interface/Fader");
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        /// <summary>
        /// Затемнение экрана.
        /// </summary>
        /// <param name="fadeInCallback"></param>
        public void FadeIn(Action fadeInCallback)
        {
            _fadeInCallback = fadeInCallback;
            animator.SetBool(name: "isFaded", false);
        }
        /// <summary>
        /// Осветление экрана.
        /// </summary>
        /// <param name="fadeOutCallback"></param>
        public void FadeOut(Action fadeOutCallback)
        {
            _fadeOutCallback = fadeOutCallback;
            animator.SetBool(name: "isFaded", true);
        }
        /// <summary>
        /// Вызов делегата _fadeInCallback после затемнения экрана.
        /// </summary>
        private void FadeInCallbackHandler()
        {
            _fadeInCallback?.Invoke();
            _fadeInCallback = null;
        }
        /// <summary>
        /// Вызов делегата _fadeOutCallback после осветления экрана.
        /// </summary>
        private void FadeOutCallbackHandler()
        {
            _fadeOutCallback?.Invoke();
            _fadeOutCallback = null;
        }
    }
}