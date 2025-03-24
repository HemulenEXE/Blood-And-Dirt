using System;
using System.Collections;
using UnityEngine;

//ќтвечает за анимацию открыти€\закрыти€ двери
public class Door : MonoBehaviour
{
    [NonSerialized]
    public bool IsOpen = false; //ќткрыта ли 
    [SerializeField]
    public enum SideOpen { Left, Right, Up, Down}; //¬ какую сторону должна открыватьс€ дверь
    public SideOpen Side; 
    private Vector3 pos; //ѕозици€ относительно которой происходит вращение
    private float speed = 40f; //—корость открыти€
    private bool isTrigger = false;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        switch (Side)
        {
            case SideOpen.Left: pos.x = transform.position.x + (GetComponent<Collider2D>().bounds.size.x / 2); break;
            case SideOpen.Right: pos.x = transform.position.x - (GetComponent<Collider2D>().bounds.size.x / 2); break;
            case SideOpen.Up: pos.y = transform.position.y + (GetComponent<Collider2D>().bounds.size.y / 2); break;
            case SideOpen.Down: pos.y = transform.position.y - (GetComponent<Collider2D>().bounds.size.y / 2); break;
        }
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(UnityEngine.Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            isTrigger = true;
        }
    }
    public void OnTriggerExit2D(UnityEngine.Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            isTrigger = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTrigger)
        {
            if (isRunning) 
            { 
                StopAllCoroutines();
                isRunning = false;
                IsOpen = !IsOpen;
            }
            if (IsOpen) StartCoroutine(Close());
            else StartCoroutine(Open());
        }
    }
    //Ёти три метода дл€ использовани€ в TimeLine'ах
    public void SetOpenSpeed(int s) { speed = s; }
    public void OpenDoor() { StartCoroutine(Open()); }
    public void CloseDoor() { StartCoroutine(Close()); }
    private IEnumerator Open() 
    {
        isRunning = true;
        Debug.Log("Start Open");
        if (Side == SideOpen.Left || Side == SideOpen.Down) {
            while (Math.Abs(360 - 90 - transform.rotation.eulerAngles.z) > 1.5)
            {
                Debug.Log($"{transform.rotation.eulerAngles.z} <-> {transform.rotation.z}");
                transform.RotateAround(pos, Vector3.back, speed * Time.deltaTime);
                yield return null;
            }
        }
        else 
        {
            while (transform.rotation.eulerAngles.z < 90)
            {
                transform.RotateAround(pos, Vector3.back, -speed * Time.deltaTime);
                yield return null;
            }
        }
        Debug.Log("EndOpen");
        isRunning = false;
        IsOpen = true;
    }
    private IEnumerator Close()
    {
        isRunning = true;
        Debug.Log("Start Close");
        if (Side == SideOpen.Left || Side == SideOpen.Down)
        {
            while (Math.Abs(0 - transform.rotation.eulerAngles.z) > 1.5)
            {
                transform.RotateAround(pos, Vector3.back, -speed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (Math.Abs(0 - transform.rotation.eulerAngles.z) > 1.5)
            {
                transform.RotateAround(pos, Vector3.back, speed * Time.deltaTime);
                yield return null;
            }
        }
        Debug.Log("End Close");
        isRunning = false;
        IsOpen = false;
    }
}
