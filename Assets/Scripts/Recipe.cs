using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Recipe
{
    [SerializeField] private readonly static int s_totAmountIngredients = 10;
    [SerializeField] private Ingredient Water = new Ingredient{Cost = 0};
    [SerializeField] private Ingredient Meat = new Ingredient{Cost = 1};
    [SerializeField] private Ingredient PlantMatter = new Ingredient{Cost = 1};

    // AddIngredient()??? by name of the ingredient?? or by its index???
    // GenerateRandomRecipe()???
    // reset to default???
    // update UIs???
}
