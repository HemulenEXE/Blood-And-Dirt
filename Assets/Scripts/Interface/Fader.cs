﻿using System;
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
        private bool fading; //идёт ли затемнение/осветление
        public static Fader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<Fader>("Prefabs/Interfaces/Fader");
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
            if (fading)
                return;

            fading = true;
            Debug.Log($"FadeIn! - {animator.GetBool("isFaded")}");
            _fadeInCallback = fadeInCallback;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetBool(name: "isFaded", true);

        }
        /// <summary>
        /// Осветление экрана.
        /// </summary>
        /// <param name="fadeOutCallback"></param>
        public void FadeOut(Action fadeOutCallback)
        {
            if (fading)
                return;

            fading = true;
            Debug.Log($"FadeOut! - {animator.GetBool("isFaded")}");
            _fadeOutCallback = fadeOutCallback;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetBool(name: "isFaded", false);
        }
        
        /// <summary>
        /// Вызов делегата _fadeInCallback после затемнения экрана.
        /// </summary>
        public void FadeInCallbackHandler()
        {
            Debug.Log("FadeInCallback вызван");
            _fadeInCallback?.Invoke();
            _fadeInCallback = null;
            fading = false;
        }
        /// <summary>
        /// Вызов делегата _fadeOutCallback после осветления экрана.
        /// </summary>
        public void FadeOutCallbackHandler()
        {
            Debug.Log("FadeOutCallback вызван");
            _fadeOutCallback?.Invoke();
            _fadeOutCallback = null;
            fading = false;
        }
    }
}