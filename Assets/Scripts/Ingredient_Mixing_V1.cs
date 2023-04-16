using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// things that could be added
// buttons that add more than one unit of material at a time
// automatic size of the ingredient array based on the number of ingedient buttons
// change structures of how ingredients are stored
    // https://stackoverflow.com/questions/31431486/how-to-get-the-gameobject-which-triggered-the-ui-button-click-function
    // https://answers.unity.com/questions/1687630/pass-more-than-one-argument-through-a-ui-button.html
    
public class Ingredient_Mixing_V1 : MonoBehaviour
{
    [SerializeField] private int cupFilledTotal = 0;
    [SerializeField] private int cupCapacity = 50;
    
    [SerializeField] private int[] ingredients = new int[4];

    public void ShowFilledStatus()
    {
        if (cupFilledTotal == cupCapacity)
        {
            Debug.Log("The Cup is full.");
        }
        else
        {
            switch(cupFilledTotal) 
            {
            case 0:
                Debug.Log("The Cup is empty.");
                break;
            case 1:
                Debug.Log("In the Cup there's 1 unit of material.");
                break;
            default:
                Debug.Log("In the Cup there are " + cupFilledTotal + " units of material.");
                break;
            }
        }
    }

    public void addIngredient(int ingredientIndex)
    {
        if (cupFilledTotal < cupCapacity)
        {
            ingredients[ingredientIndex]++;
            cupFilledTotal++;
            Debug.Log("Added one of ingredient " + (ingredientIndex + 1));
        }
    }
}
