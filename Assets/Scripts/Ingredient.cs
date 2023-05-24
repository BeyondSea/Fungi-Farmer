using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class Ingredient
{
    public string Name;
    public int Amount;
    public int Cost;

    public Ingredient()
    {
        Name = "Unassigned";
        Amount = 0;
        Cost = -1;
    }
}