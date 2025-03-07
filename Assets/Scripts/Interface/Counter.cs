using UnityEngine;

//Счётчик игровых очков
public class Counter
{
    private int points;
    private static Counter instance;
    public static Counter Instance() 
    {
        if (instance == null)
            instance = new Counter();
        return instance;
    }
    public int Points() { return points; }
    //Добавление очков
    public void AddPoints(int count) { points += count; }
    //Вычитание очков
    public void RemovePoints(int count) 
    {
        if (points - count == 0)
            points = 0; 
        else 
            points -= count; 
    } 
}
