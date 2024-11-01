using System;
using UnityEngine;

/// <summary>
/// Класс Fader отвечает за затемнение и осветления экрана
/// </summary>
public class Fader : MonoBehaviour
{
    private static Fader _instance;

    [SerializeField] 
    private Animator animator;

    private Action _fadeInCallback;
    private Action _fadeOutCallback;
    public static Fader Instance
    {
        get { 
            if (_instance == null)
            {
                var prefab = Resources.Load<Fader>("Fader");
                _instance = Instantiate(prefab);
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    /// <summary>
    /// Затемняет экран
    /// </summary>
    /// <param name="fadeInCallback"></param>
    public void FadeIn(Action fadeInCallback)
    {
        _fadeInCallback = fadeInCallback;
        animator.SetBool(name: "isFaded", false);
    }
    /// <summary>
    /// Осветляет экран
    /// </summary>
    /// <param name="fadeOutCallback"></param>
    public void FadeOut(Action fadeOutCallback)
    {
        _fadeOutCallback = fadeOutCallback;
        animator.SetBool(name: "isFaded", true);
    }
    /// <summary>
    /// Вызывает указанный делегат после затемнения экрана
    /// </summary>
    private void FadeInCallbackHandler()
    {
        _fadeInCallback?.Invoke();
        _fadeInCallback = null;
    }
    /// <summary>
    /// Вызывает указанный делегат после осветления экрана
    /// </summary>
    private void FadeOutCallbackHandler()
    {
        _fadeOutCallback?.Invoke();
        _fadeOutCallback = null;
    }
}
