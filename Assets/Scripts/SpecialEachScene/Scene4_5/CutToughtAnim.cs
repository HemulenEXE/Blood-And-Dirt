using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//Контролирует включение завершающей анимации и её состоянии
public class CutToughtAnim : MonoBehaviour
{
    [NonSerialized]
    public bool EndAnim = false;
    private bool flag = true;

    Animator animator;
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && flag)
        {
            animator.enabled = true;
            animator.SetBool("CutTought", true);
            collision.gameObject.SetActive(false);
            Transform muller = this.transform.parent;
            muller.rotation = new Quaternion(0, 0, 0, 0);
            muller.position = new Vector3(muller.position.x, 35.28f, 0);
            StartCoroutine(Wait());
            flag = false;
        }
    }
    private IEnumerator Wait()
    {
        GameObject.Find("InteractiveUI").GetComponent<InteractiveUI>().TurnOffText();
        var animInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName("гг отрубает язык_Clip"))
            yield return new WaitForSeconds(animInfo.length);
        
        EndAnim = true;
        yield return new WaitForFixedUpdate();
        this.gameObject.SetActive(false);
    }
}
