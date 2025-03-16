using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public readonly GameObject unit;
    public readonly int costUnit;

    public Unit(GameObject unit, int costUnit)
    {
        this.unit = unit;
        this.costUnit = costUnit;
    }   
}
