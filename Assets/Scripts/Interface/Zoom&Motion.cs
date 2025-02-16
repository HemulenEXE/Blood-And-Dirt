using UnityEngine;

//Скрипт навешивается на компонент Tree. Управляет его перемещением и масштабированием
public class ZoomAndMotion : MonoBehaviour
{
    [SerializeField]
    private float maxZoom;
    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float radius; //радиус, в котором можно перемещать дерево
    private Vector2 startPoint; //Исходная позиция и масштаб
    private Vector2 startScale;
    private Vector2 pos; 
    private Vector2 offset; 
    private Vector2 mousePos;
    public Vector2 GStartPoint() {  return startPoint; }
    public Vector2 StartScale() { return startScale; }
    private void Start()
    {
        startScale = this.transform.localScale;
        startPoint = this.transform.position;
    }
    void Update()
    {
        //Фиксация расстояния, на котором друг от друга должны находиться курсор мыши и дерево
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {   
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            pos = this.transform.position;
            offset = mousePos - pos;
        } //Перемещение дерева по траектории курсора
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            float x = this.transform.position.x, y = this.transform.position.y;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            if ((Mathf.Abs(mousePos.y - offset.y - startPoint.y) <= radius))
                y = mousePos.y - offset.y; 
            if ((Mathf.Abs(mousePos.x - offset.x - startPoint.x) <= radius))
                x = mousePos.x - offset.x;
            this.transform.position = new Vector2(x, y);
            pos = this.transform.position;
        }
        //увелечение\уменьшение дерева
        if ( this.transform.localScale.x -Input.GetAxis("Mouse ScrollWheel") >= minZoom && this.transform.localScale.x - Input.GetAxis("Mouse ScrollWheel") <= maxZoom)
            this.transform.localScale += new Vector3(-Input.GetAxis("Mouse ScrollWheel"), -Input.GetAxis("Mouse ScrollWheel"));
    }
}
