using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class Ingredient
{
    public int Amount;
    public int Cost;

    public Ingredient()
    {
        Amount = 0;
        Cost = -1;
    }
}