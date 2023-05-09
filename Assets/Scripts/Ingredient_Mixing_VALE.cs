using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// things that could be added
// buttons that add more than one unit of material at a time
// automatic size of the ingredient array based on the number of ingedient buttons
// change structures of how ingredients are stored
    // https://stackoverflow.com/questions/31431486/how-to-get-the-gameobject-which-triggered-the-ui-button-click-function
    // https://answers.unity.com/questions/1687630/pass-more-than-one-argument-through-a-ui-button.html

// Promemoria: Random.Range(min, max) quando è usato con gli int è INCLUSIVO per quanto riguarda il valore minimo
// e ESCLUSIVO per quanto riguarda il valore massimo

public class Ingredient_Mixing_VALE : MonoBehaviour
{
    #region Variables
    [SerializeField] static private int numIngredients = 3;
    // ingredients index list
    // 0: Water
    // 1: Meat
    // 2: Plant Matter
    // (3: Fertilizer)

    // Cup Settings
    [SerializeField] static private int cupCapacity = 10;
    [SerializeField] private int cupFilledTotal = 0;
    [SerializeField] private TMP_Text cupFilledText;
    public Slider slider;

    // Rats settings
    //[SerializeField] static private int numberOfRats = 2;
    [SerializeField] public int animalsLeft = 4;
    [SerializeField] private TMP_Text numOfRatsText;
    [SerializeField] private TMP_Text TestCommentText;
    
    //Test Dialogues
    [SerializeField] [TextArea(1,5)] public string failedTestDialogue;
    [SerializeField] [TextArea(3,5)] public string[] ratDialogue;
    [SerializeField] [TextArea(3,5)] public string[] catDialogue;
    [SerializeField] [TextArea(3,5)] public string[] dogDialogue;
    [SerializeField] [TextArea(3,5)] public string[] youDialogue;

    //Buttons
    [SerializeField] public Button ratTestButton;
    [SerializeField] public Button catTestButton;
    [SerializeField] public Button dogTestButton;
    [SerializeField] public Button youTestButton;
    [SerializeField] public Button passNightButton;
    [SerializeField] public Button moldMadeOkButton;

    // Correct Recipe
    [SerializeField] private int[] correctRecipe = new int[numIngredients];
    [SerializeField] private bool isPositiveOrNegative;   // true means that is a positive recipe, false means that it's negative
    [SerializeField] private TMP_Text requestDescr;
    [SerializeField] private TMP_Text[] CorrectRecipeScoreText = new TMP_Text[numIngredients];

    // Player Recipe
    [SerializeField] private int[] playerRecipe = new int[numIngredients];
    [SerializeField] private TMP_Text[] playerRecipeText = new TMP_Text[numIngredients];
    [SerializeField] private TMP_Text[] playerScoreText = new TMP_Text[numIngredients];

    // Canvases
    [SerializeField] private GameObject IngredientMixingCanvas;
    [SerializeField] private GameObject NewDayCanvas;
    [SerializeField] private GameObject TestResultCanvas;
    [SerializeField] private GameObject ScoreCanvas;
    [SerializeField] private GameObject TestResultTextBox;
    [SerializeField] private GameObject requestCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject moldMadeCanvas;

    //Test Subjects Images
    [SerializeField] public Image ratImage;
    [SerializeField] public Image catImage;
    [SerializeField] public Image dogImage;
    [SerializeField] public Image youImage;
    [SerializeField] public Sprite emptyImage;

    //Test Subjects Options
    [SerializeField] public GameObject ratOption;
    [SerializeField] public GameObject catOption;
    [SerializeField] public GameObject dogOption;
    [SerializeField] public GameObject youOption; 

    //Sound effects
    [SerializeField] public AudioSource ratAudio;
    [SerializeField] public AudioSource catAudio;
    [SerializeField] public AudioSource dogAudio;
    [SerializeField] public AudioSource youAudio;

    //Created mold
    [SerializeField] public Image yourMoldImage;

    #endregion
    
// Start is called before the first frame update
    void Start()
    {
        GenerateRecipe();
        GenerateRecipeDescription();

        passNightButton.interactable = false;

        //numOfRatsText.text = numberOfRats.ToString();
    }

    // Presumo che sia necessario che le ricette abbiano sempre
    // almemeno un'unità di acqua e almeno un'unità di carne o materiale vegetale.
    // Presumo inoltre che possano essere di due tipi soltanto
    // positive: se contengono materiale vegetale
    // negative: se contengono carne
    // e che di conseguenza le ricette non possano avere sia carne che materiale vegetale.
    // Infine presumo che le ricette non debbano per forza riempire il recipiente, ma che possano anche avere meno unità di ingredienti
    private void GenerateRecipe()
    {
        // Decide whether the recipe is positive or negative
        int temp = Random.Range(0, 2);
        if (temp == 0)
        {
            isPositiveOrNegative = false;
        }
        else {isPositiveOrNegative = true;}

        // Decide amount of Ingredients
        int sum = 0;

        // Activator (Plant Matter or Meat)
        // Has to be at least 1 but not the same as the limit
        // as there must be at least 1 water
        if (isPositiveOrNegative == false)
        {
            correctRecipe[1] = Random.Range(1, cupCapacity);
        }
        else
        {
            correctRecipe[2] = Random.Range(1, cupCapacity);
        }
        sum += correctRecipe[1];
        sum += correctRecipe[2];

        // Water
        // Has to be at least 1 
        correctRecipe[0] = Random.Range(1, cupCapacity-sum+1);
        sum += correctRecipe[0];

        /*
        // Fertilizzante
        // Can be any value
        correctRecipe[3] = Random.Range(0, cupCapacity-sum+1);
        sum += correctRecipe[0];
        */
    }

    private void GenerateRecipeDescription()
    {
        requestDescr.text = "The mold must be ";

        //Acqua
        if (correctRecipe[0] > 5)
        {
            requestDescr.text += "a little ";
        }
        else if (correctRecipe[0] < 5)
        {
            requestDescr.text += "very ";
        }
        else if (correctRecipe[0] == 5)
        {
            requestDescr.text += "somewhat ";
        }

        //Materiale organico
        if (isPositiveOrNegative == false)
        {
            requestDescr.text += "poisonius.";
        }
        else
        {
            requestDescr.text += "healthy.";
        }
        
        /* Type
        requestDescr.text = "Vorrei una muffa ";
        if (isPositiveOrNegative == false)
        {
            requestDescr.text += "velenosa";
        }
        else
        {
            requestDescr.text += "curativa";
        }
        
        // Speed -> amount of activator
        if (correctRecipe[1]>6 || correctRecipe[2]>6)
        {
            requestDescr.text += " che agisca velocemente.\n";
        }
        else if (correctRecipe[1]<4 || correctRecipe[2]<4)
        {
            requestDescr.text += " che agisca lentamente.\n";
        }*/
        
        // Water -> ???

        /*
        // Quantity -> amount of Fertilizer
        if (correctRecipe[3]>6)
        {
            requestDescr.text += "Devo farne una bella scorta, quindi me ne servirà un po'.";
        }
        else if (correctRecipe[3]<4)
        {
            requestDescr.text += "Non me ne serve tanta, probabilmente solo una dose.";
        }
        */
    }

    public void AddIngredient(int ingredientIndex)
    {
        if (cupFilledTotal < cupCapacity)
        {
            playerRecipe[ingredientIndex]++;
            cupFilledTotal++;

            playerRecipeText[ingredientIndex].text = playerRecipe[ingredientIndex].ToString();
            cupFilledText.text = "Total: " + cupFilledTotal.ToString() + "/" + cupCapacity;
            slider.value = cupFilledTotal;
        }

        if (cupFilledTotal == cupCapacity)
        {
            passNightButton.interactable = true;
        }
    }

    public void PassNight()
    {
        // Animazione del passaggio della notte?
        
        requestCanvas.SetActive(false);
        IngredientMixingCanvas.SetActive(false);
        moldMadeCanvas.SetActive(true);

        moldMadeOkButton.interactable = true;

        /* mostra altro canvas
        IngredientMixingCanvas.SetActive(false);
        NewDayCanvas.SetActive(true);

        // Disattiva bottone notte
        passNightButton.interactable = false;

        if (animalsLeft > 0)
        {
            ratTestButton.interactable = true;
            catTestButton.interactable = true;
            dogTestButton.interactable = true;
            youTestButton.interactable = true;
        }

        if (animalsLeft == 4)
        {
            ratOption.SetActive(true);
        }
        else if (animalsLeft == 3)
        {
            ratOption.SetActive(false);
            catOption.SetActive(true);
        }
        else if (animalsLeft == 2)
        {
            catOption.SetActive(false);
            dogOption.SetActive(true);

        }
        else if (animalsLeft == 1)
        {
            dogOption.SetActive(false);
            youOption.SetActive(true);
        }*/
    }

    public void MoldMadeOk()
    {
        // mostra altro canvas
        requestCanvas.SetActive(true);
        moldMadeCanvas.SetActive(false);
        NewDayCanvas.SetActive(true);

        // Disattiva bottone notte
        passNightButton.interactable = false;

        if (animalsLeft > 0)
        {
            ratTestButton.interactable = true;
            catTestButton.interactable = true;
            dogTestButton.interactable = true;
            youTestButton.interactable = true;
        }

        if (animalsLeft == 4)
        {
            ratOption.SetActive(true);
        }
        else if (animalsLeft == 3)
        {
            ratOption.SetActive(false);
            catOption.SetActive(true);
        }
        else if (animalsLeft == 2)
        {
            catOption.SetActive(false);
            dogOption.SetActive(true);

        }
        else if (animalsLeft == 1)
        {
            dogOption.SetActive(false);
            youOption.SetActive(true);
        }
    }

    public void TestResult()
    {
        if (playerRecipe[1] > 0 && playerRecipe[2] > 0)
        {
            TestCommentText.text = "";
            TestCommentText.text = failedTestDialogue;
        }

        else if (animalsLeft == 4)
        {
            animalsLeft--;

            ratTestButton.interactable = false;
            ratImage.sprite = emptyImage;
            
            //If rat is left, and recipe has meat
            //then if rat is left, and recipe has vegs
            if (playerRecipe[1] > correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[5];
            }
            
        }

        else if (animalsLeft == 3)
        {
            animalsLeft--;

            catTestButton.interactable = false;
            catImage.sprite = emptyImage;
            
            //same, if cat
            if (playerRecipe[1] > correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[5];
            }
        }

        else if (animalsLeft == 2)
        {
            animalsLeft--;

            dogTestButton.interactable = false;
            dogImage.sprite = emptyImage;
           
           //same, if dog
            if (playerRecipe[1] > correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[5];
            }
        }

        else if (animalsLeft == 1)
        {
            animalsLeft--;

            youTestButton.interactable = false;
            youImage.sprite = emptyImage;

           //same, if you
            if (playerRecipe[1] > correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[0];
                YouDie();
            }
            else if (playerRecipe[1] < correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[1];
                YouDie();
            }
            else if (playerRecipe[1] == correctRecipe[1] && isPositiveOrNegative == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[2];
                YouDie();
            }
            else if (playerRecipe[2] > correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[3];
                YouDie();
            }
            else if (playerRecipe[2] < correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2] && isPositiveOrNegative == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[5];
            }
        }
    }

    public void GoBackToPassNight()
    {
        // mostra altro canvas
        TestResultCanvas.SetActive(false);
        NewDayCanvas.SetActive(true);
    }

    public void ThrowAwayMold()
    {
        //reset della muffa
        for (int j=0; j<numIngredients; j++)
        {
            playerRecipe[j] = 0;
            playerRecipeText[j].SetText("0");
        }

        cupFilledText.text = "Total: 0/10";
        cupFilledTotal = 0;
        slider.value = cupFilledTotal;

        // ritorna all'altro canvas
        NewDayCanvas.SetActive(false);
        IngredientMixingCanvas.SetActive(true);
    }

    public void GiveToClient()
    {
        // mostra altro canvas
        NewDayCanvas.SetActive(false);
        requestCanvas.SetActive(false);
        //ScoreCanvas.SetActive(true);

        if (playerRecipe[0] == correctRecipe[0] && playerRecipe[1] == correctRecipe[1] && playerRecipe[2] == correctRecipe[2])
        {
            winCanvas.SetActive(true);
        }
        else
        {
            ScoreCanvas.SetActive(true);

            // show correct recipe
            for (int k=0; k<numIngredients; k++)
            {
                CorrectRecipeScoreText[k].text = correctRecipe[k].ToString();
            }

            // show player recipe
            for (int m=0; m<numIngredients; m++)
            {
                playerScoreText[m].text = playerRecipe[m].ToString();
            }
        }
    }

    public void YouDie()
    {
        NewDayCanvas.SetActive(false);
        requestCanvas.SetActive(false);
        loseCanvas.SetActive(true);
    }

    void Update()
    {
        //Debug.Log("animals left" +animalsLeft);
    }

    public void PlaySoundEffect()
    {
        ratAudio.Play(0);
        catAudio.Play(0);
        dogAudio.Play(0);
        youAudio.Play(0);
    }
}
