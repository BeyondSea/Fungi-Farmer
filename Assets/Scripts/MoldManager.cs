using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private int cupFilledTotal = 0;
    public Recipe correctRecipe = new Recipe();
    public Recipe playerRecipe = new Recipe();
    #endregion

    void Start()
    {
        
    }
}
