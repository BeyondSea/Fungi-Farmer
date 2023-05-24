using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Recipe
{
    [SerializeField] private readonly static int s_totAmountIngredients = 10;

    // Types of Ingredients
    [SerializeField] private Ingredient[] _listIngredients = new Ingredient[]
    {
        // Water
        new Ingredient
        {
            Name = "Water",
            Cost = 0
        },
        
        // Meat
        new Ingredient
        {
            Name = "Meat",
            Cost = 1
        },

        // Plant Matter
        new Ingredient
        {
            Name = "Plant Matter",
            Cost = 1
        }
    };

    // AddIngredient()??? by name of the ingredient?? or by its index???
    // GenerateRandomRecipe()???
    // reset to default???
    // update UIs???
}
