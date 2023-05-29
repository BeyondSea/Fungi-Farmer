using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Copia_Script : MonoBehaviour
{
    #region Variables
    [SerializeField] static private int numIngredients = 3;
    // ingredients index list
    // 0: Water
    // 1: Meat
    // 2: Plant Matter
    // (3: Fertilizer)
    //[SerializeField] private Color[] ingredientColors;
    [SerializeField] private Color moldColor;
    [SerializeField] private Color beakerColor;
    [SerializeField] public Image beakerImage;
    [SerializeField] public Sprite[] beakerSprites = new Sprite[11];

    // Cup Settings
    [SerializeField] static private int cupCapacity = 10;
    [SerializeField] private int cupFilledTotal = 0;
    [SerializeField] private TMP_Text cupFilledText;

    // Coins
    [SerializeField] private int coins = 20;
    [SerializeField] private TMP_Text[] coinsCounter = new TMP_Text[3];
    [SerializeField] private GameObject buyRatGameObject;
    [SerializeField] private int ratCost = 1;
    [SerializeField] public TMP_Text coinsEarned;
    
    // Correct Recipe
    [SerializeField] private int[] correctRecipe = new int[numIngredients];
    [SerializeField] private bool isPositive;   // true means that is a positive recipe, false means that it's negative
    //[SerializeField] private TMP_Text requestDescr;
    [SerializeField] private TMP_Text[] CorrectRecipeScoreText = new TMP_Text[numIngredients];

    //Request
    [SerializeField] public TMP_Text recipeRequestText;
    [SerializeField] [TextArea(2,5)] public string[] recipeRequestDialogue; 
    [SerializeField] public Image clientPortrait;
    [SerializeField] public Sprite[] clientPortraitSprites;

    // Player Recipe
    [SerializeField] private int[] playerRecipe = new int[numIngredients];
    [SerializeField] private TMP_Text[] playerRecipeText = new TMP_Text[numIngredients];
    [SerializeField] private TMP_Text[] playerScoreText = new TMP_Text[numIngredients];

    // Canvases
    [SerializeField] private GameObject IngredientMixingCanvas;
    [SerializeField] private GameObject NewDayCanvas;
    [SerializeField] private GameObject ScoreCanvas;
    [SerializeField] private GameObject TestResultTextBox;
    [SerializeField] private GameObject requestCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject moldMadeCanvas;
    [SerializeField] private GameObject testCommentsCanvas;
    [SerializeField] private GameObject gameStart;

    //Buttons
    [SerializeField] public Button ratTestButton;
    [SerializeField] public Button catTestButton;
    [SerializeField] public Button dogTestButton;
    [SerializeField] public Button youTestButton;
    [SerializeField] public Button passNightButton;
    [SerializeField] public Button moldMadeOkButton;
    [SerializeField] public Button buyRatButton;
    [SerializeField] public Button backToMoldActions;
    [SerializeField] public Button startGame;

    //Test Dialogues
    [SerializeField] [TextArea(1,5)] public string failedTestDialogue;
    [SerializeField] [TextArea(3,5)] public string[] ratDialogue;
    [SerializeField] [TextArea(3,5)] public string[] catDialogue;
    [SerializeField] [TextArea(3,5)] public string[] dogDialogue;
    [SerializeField] [TextArea(3,6)] public string[] youDialogue;
    [SerializeField] [TextArea(3,6)] public string[] youLoseDialogue;
    [SerializeField] [TextArea(3,5)] public string[] testHintDialogue;

    // Test subjects details
    [SerializeField] public int animalsLeft = 4;
    [SerializeField] public bool ratBought = false;
    [SerializeField] private TMP_Text TestCommentText;
    [SerializeField] public TMP_Text testHintText;

    //Test Subjects Images
    [SerializeField] public Image ratImage;
    [SerializeField] public Image catImage;
    [SerializeField] public Image dogImage;
    [SerializeField] public Image youImage;
    [SerializeField] public Sprite emptyImage;
    [SerializeField] public Sprite originalRatImage;

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
    [SerializeField] public Image moldMadeImage;
    [SerializeField] public Image yourMoldImage;
    [SerializeField] public Image yourMoldFace;
    [SerializeField] public Sprite yourMoldFaceHealthy;
    [SerializeField] public Sprite yourMoldFacePoison;

    //Result screens
    [SerializeField] public TMP_Text sentMoldCommentText;
    [SerializeField] [TextArea(3,5)] public string[] sentMoldCommentDialogue; //0=0 coins recieved
    [SerializeField] public TMP_Text youLoseText;
    public int typeOfDeath; //0=no money
    [SerializeField] public TMP_Text gameOverText;
    [SerializeField] [TextArea(1,5)] public string youWinDialogue;
    [SerializeField] public TMP_Text youWinText;


    #endregion
    
    void Start()
    {
        gameStart.SetActive(true);
        GenerateRecipe();
        //GenerateRecipeDescription();
        RecipeRequest();

        passNightButton.interactable = false;
        RefreshCoinsCounters();
        // animalsBeforeBuying = animalsLeft;
    }

    public void StartGame()
    {
        gameStart.SetActive(false);
        requestCanvas.SetActive(true);
        IngredientMixingCanvas.SetActive(true);
    }

    // Presumo che sia necessario che le ricette abbiano sempre
    // almemeno un'unità di acqua e almeno un'unità di carne o materiale vegetale.
    // Presumo inoltre che possano essere di due tipi soltanto
    // positive: se contengono materiale vegetale
    // negative: se contengono carne
    // e che di conseguenza le ricette non possano avere sia carne che materiale vegetale.
    // Infine presumo che le ricette debbano per forza riempire il recipiente.

    private void GenerateRecipe()
    {
        Debug.Log("Generating Recipe");
        
        // Clear memory
        for (int k=0; k<numIngredients; k++)
        {
            correctRecipe[k] = 0;
        }

        // Decide whether the recipe is positive or negative
        int temp = Random.Range(0, 2);
        if (temp == 0)
        {
            isPositive = true;
        }
        else {isPositive = false;}

        // Activator (Plant Matter or Meat)
        // Has to be at least 1 but not the same as the limit
        // as there must be at least 1 water
        if (isPositive == false)
        {
            correctRecipe[1] = Random.Range(1, cupCapacity);
        }
        else
        {
            correctRecipe[2] = Random.Range(1, cupCapacity);
        }

        // Water
        // Has to be at least 1 
        correctRecipe[0] = cupCapacity - correctRecipe[1] - correctRecipe[2];
    }

    /*private void GenerateRecipeDescription()
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
        if (isPositive == false)
        {
            requestDescr.text += "poisonius.";
        }
        else
        {
            requestDescr.text += "healthy.";
        }
    }*/

    public void RecipeRequest()
    {
        Debug.Log("RecipeRequesting");

        if (correctRecipe[1] >= 7 && isPositive == false)
        {
            recipeRequestText.text = "";
            recipeRequestText.text = recipeRequestDialogue[0];
            clientPortrait.sprite = clientPortraitSprites[0];
        }
        else if (correctRecipe[1] >= 4 && correctRecipe[1] < 7 && isPositive == false)
        {
            recipeRequestText.text = "";
            recipeRequestText.text = recipeRequestDialogue[1];
            clientPortrait.sprite = clientPortraitSprites[1];
        }
        else if (correctRecipe[1] < 4 && isPositive == false)
        {
            recipeRequestText.text = "";
            recipeRequestText.text = recipeRequestDialogue[2];
            clientPortrait.sprite = clientPortraitSprites[2];
        }
        else if (correctRecipe[2] >= 7 && isPositive == true)
        {
            recipeRequestText.text = "";
            recipeRequestText.text = recipeRequestDialogue[3];
            clientPortrait.sprite = clientPortraitSprites[3];
        }
        else if (correctRecipe[2] >= 4 && correctRecipe[2] < 7 && isPositive == true)
        {
            recipeRequestText.text = "";
            recipeRequestText.text = recipeRequestDialogue[4];
            clientPortrait.sprite = clientPortraitSprites[4];
        }
        else if (correctRecipe[2] < 4 && isPositive == true)
        {
            recipeRequestText.text = "";
            recipeRequestText.text = recipeRequestDialogue[5];
            clientPortrait.sprite = clientPortraitSprites[5];
        }
        
    }

    public void AddIngredient(int ingredientIndex)
    {
        if (cupFilledTotal < cupCapacity)
        {
            int cost = GetCost(ingredientIndex);
            if (cost <= coins)
            {
                coins -= cost;
                playerRecipe[ingredientIndex]++;
                cupFilledTotal++;

                playerRecipeText[ingredientIndex].text = playerRecipe[ingredientIndex].ToString();
                cupFilledText.text = "Total: " + cupFilledTotal.ToString() + "/" + cupCapacity;
                RefreshCoinsCounters();

                ChangeBeakerSprite();
                ChangeBeakerColor();
            }        
        }

        if (cupFilledTotal == cupCapacity)
        {
            passNightButton.interactable = true;
        }
    }

    public void PassNight()
    {        
        requestCanvas.SetActive(false);
        IngredientMixingCanvas.SetActive(false);
        moldMadeCanvas.SetActive(true);

        moldMadeOkButton.interactable = true;

        //Prendi quantità ingredienti e trasformali in colore, poi applicalo
        moldColor[2] = playerRecipe[0];
        moldColor[0] = playerRecipe[1];
        moldColor[1] = playerRecipe[2];
        moldColor[3] = 1;

        moldMadeImage.GetComponent<Image>().color = moldColor;

        //Scambia faccia della muffa
        if (playerRecipe[1] > 0)
        {
            yourMoldFace.sprite = yourMoldFacePoison;
        }
        else
        {
            yourMoldFace.sprite = yourMoldFaceHealthy;
        }
    }

    public void MoldMadeOk()
    {
        // mostra altro canvas
        requestCanvas.SetActive(true);
        moldMadeCanvas.SetActive(false);
        NewDayCanvas.SetActive(true);

        // Disattiva bottone notte
        passNightButton.interactable = false;

        //Resetta test textbox
        TestCommentText.text = "";
        
        //Se mostra opzioni di test
        if ( (animalsLeft < 4) && (ratCost <= coins) )
        {
            buyRatGameObject.SetActive(true);
            buyRatButton.interactable = true;
        }

        if (animalsLeft > 0)
        {
            ratTestButton.interactable = true;
            catTestButton.interactable = true;
            dogTestButton.interactable = true;
            youTestButton.interactable = true;
        }

        //Quale opzione di test mostra
        if (animalsLeft == 4 || ratBought)
        {
            ratOption.SetActive(true);
            catOption.SetActive(false);
            dogOption.SetActive(false);
            youOption.SetActive(false);
        }
        else if (animalsLeft == 3)
        {
            ratOption.SetActive(false);
            catOption.SetActive(true);
            dogOption.SetActive(false);
            youOption.SetActive(false);
        }
        else if (animalsLeft == 2)
        {
            ratOption.SetActive(false);
            catOption.SetActive(false);
            dogOption.SetActive(true);
            youOption.SetActive(false);
        }
        else if (animalsLeft == 1)
        {
            ratOption.SetActive(false);
            catOption.SetActive(false);
            dogOption.SetActive(false);
            youOption.SetActive(true);
        }

        //Colore della muffa
        yourMoldImage.GetComponent<Image>().color = moldColor;
    }

    public void TestResult()
    {
        //Canvases
        //NewDayCanvas.SetActive(false);
        //requestCanvas.SetActive(false);
        testCommentsCanvas.SetActive(true);

        buyRatButton.interactable = false;

        // ricetta mista
        if (playerRecipe[1] > 0 && playerRecipe[2] > 0)
        {
            TestCommentText.text = "";
            TestCommentText.text = failedTestDialogue;
        }
        // si ha un ratto comprato
        else if (ratBought)
        {
            ratTestButton.interactable = false;
            ratImage.sprite = emptyImage;
            ratBought = false;
            
            if (playerRecipe[1] >= 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[0];
            }
            else if (playerRecipe[1] >= 4 && playerRecipe[1] < 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[1];
            }
            else if (playerRecipe[1] < 4 && playerRecipe[1] > 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[2];
            }
            else if (playerRecipe[2] >= 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[3];
            }
            else if (playerRecipe[2] >= 4 && playerRecipe[2] < 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[4];
            }
            else if (playerRecipe[2] < 4 && playerRecipe[2] > 0 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[5];
            }
            else if (playerRecipe[1] == 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[6];
            }
        }

        else if (animalsLeft == 4)
        {
            animalsLeft--;

            ratTestButton.interactable = false;
            ratImage.sprite = emptyImage;
            
            //If rat is left, and recipe has meat
            //then if rat is left, and recipe has vegs
            if (playerRecipe[1] >= 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[0];
            }
            else if (playerRecipe[1] >= 4 && playerRecipe[1] < 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[1];
            }
            else if (playerRecipe[1] < 4 && playerRecipe[1] > 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[2];
            }
            else if (playerRecipe[2] >= 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[3];
            }
            else if (playerRecipe[2] >= 4 && playerRecipe[2] < 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[4];
            }
            else if (playerRecipe[2] < 4 && playerRecipe[2] > 0 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[5];
            }
            else if (playerRecipe[1] == 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = ratDialogue[6];
            }
        }

        else if (animalsLeft == 3)
        {
            animalsLeft--;

            catTestButton.interactable = false;
            catImage.sprite = emptyImage;
            
            //same, if cat
            if (playerRecipe[1] >= 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[0];
            }
            else if (playerRecipe[1] >= 4 && playerRecipe[1] < 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[1];
            }
            else if (playerRecipe[1] < 4 && playerRecipe[1] > 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[2];
            }
            else if (playerRecipe[2] >= 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[3];
            }
            else if (playerRecipe[2] >= 4 && playerRecipe[2] < 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[4];
            }
            else if (playerRecipe[2] < 4 && playerRecipe[2] > 0 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[5];
            }
            else if (playerRecipe[1] == 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = catDialogue[6];
            }
        }

        else if (animalsLeft == 2)
        {
            animalsLeft--;

            dogTestButton.interactable = false;
            dogImage.sprite = emptyImage;
           
           //same, if dog
            if (playerRecipe[1] >= 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[0];
            }
            else if (playerRecipe[1] >= 4 && playerRecipe[1] < 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[1];
            }
            else if (playerRecipe[1] < 4 && playerRecipe[1] > 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[2];
            }
            else if (playerRecipe[2] >= 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[3];
            }
            else if (playerRecipe[2] >= 4 && playerRecipe[2] < 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[4];
            }
            else if (playerRecipe[2] < 4 && playerRecipe[2] > 0 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[5];
            }
            else if (playerRecipe[1] == 0 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[6];
            }
        }

        else if (animalsLeft == 1)
        {
            animalsLeft--;

            youTestButton.interactable = false;
            youImage.sprite = emptyImage;

           //same, if you
            if (playerRecipe[1] >= 7 && playerRecipe[2] == 0)
            {
                typeOfDeath = 1;
                YouDie();
            }
            else if (playerRecipe[1] >= 4 && playerRecipe[1] < 7 && playerRecipe[2] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[1];
            }
            else if (playerRecipe[1] < 4 && playerRecipe[1] > 0 && playerRecipe[2] == 0)
            {
                typeOfDeath = 2;
                YouDie();
            }
            else if (playerRecipe[2] >= 7 && playerRecipe[1] == 0)
            {
                TestCommentText.text = "";
                TestCommentText.text = youDialogue[3];
            }
            else if (playerRecipe[2] >= 4 && playerRecipe[2] < 7 && playerRecipe[1] == 0)
            {
                typeOfDeath = 3;
                YouDie();
            }
            else if (playerRecipe[2] < 4 && playerRecipe[2] > 0 && playerRecipe[1] == 0)
            {
                typeOfDeath = 4;
                YouDie();
            }
            else if (playerRecipe[1] == 0 && playerRecipe[2] == 0)
            {
                typeOfDeath = 5;
                YouDie();
            }
        }
        
        //Per dopo
            /*if (playerRecipe[1] > correctRecipe[1] && isPositive == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1] && isPositive == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1] && isPositive == false)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2] && isPositive == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2] && isPositive == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2] && isPositive == true)
            {
                TestCommentText.text = "";
                TestCommentText.text = dogDialogue[5];
            }*/
        
        //Controllo accuratezza per Hints
        if (playerRecipe[1] == 0 && playerRecipe[2] == 0)
        {
            testHintText.text = "";
            testHintText.text = testHintDialogue[0];
        }
        else if (( isPositive && ( playerRecipe[1] > 0 )) || ( !isPositive && ( playerRecipe[2] > 0 )))
        {
            testHintText.text = "";
            testHintText.text = testHintDialogue[1];
        }
        else if (( playerRecipe[1] - correctRecipe[1] == 4 ) || ( playerRecipe[1] - correctRecipe[1] == -4 ) || ( playerRecipe[2] - correctRecipe[2] == 4 ) || ( playerRecipe[2] - correctRecipe[2] == -4 ))
        {
            testHintText.text = "";
            testHintText.text = testHintDialogue[2];
        }
        else if (( playerRecipe[1] - correctRecipe[1] == 2 ) || ( playerRecipe[1] - correctRecipe[1] == -2 ) || ( playerRecipe[2] - correctRecipe[2] == 2 ) || ( playerRecipe[2] - correctRecipe[2] == -2 ))
        {
            testHintText.text = "";
            testHintText.text = testHintDialogue[3];
        }
        else if (playerRecipe[0] == correctRecipe[0] && playerRecipe[1] == correctRecipe[1] && playerRecipe[2] == correctRecipe[2])
        {
            testHintText.text = "";
            testHintText.text = testHintDialogue[4];
        }
        else
        {
            testHintText.text = "";
            testHintText.text = testHintDialogue[5];
        }
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

        ChangeBeakerSprite();
        ChangeBeakerColor();

        // ritorna all'altro canvas
        NewDayCanvas.SetActive(false);
        IngredientMixingCanvas.SetActive(true);
    }

    public void GiveToClient()
    {
        // mostra altro canvas
        NewDayCanvas.SetActive(false);
        requestCanvas.SetActive(false);
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
        
        //Coins Score
        if (playerRecipe[0] == correctRecipe[0] && playerRecipe[1] == correctRecipe[1] && playerRecipe[2] == correctRecipe[2])
        {
            coins += 5;
            coinsEarned.text = "+5 Coins";

            sentMoldCommentText.text = "";
            sentMoldCommentText.text = sentMoldCommentDialogue[3];
        }
        else if ( ( isPositive && ( playerRecipe[1] > 0 ) ) || ( !isPositive && ( playerRecipe[2] > 0 ) ) )
        {
            coins += 0;
            coinsEarned.text = "+0 Coins";
            
            sentMoldCommentText.text = "";
            sentMoldCommentText.text = sentMoldCommentDialogue[0];
        }
        else if ( ( playerRecipe[1] - correctRecipe[1] == 1 ) || ( playerRecipe[1] - correctRecipe[1] == -1 ) || ( playerRecipe[2] - correctRecipe[2] == 1 ) || ( playerRecipe[2] - correctRecipe[2] == -1 ) )
        {
            coins += 3;
            coinsEarned.text = "+3 Coins";
            
            sentMoldCommentText.text = "";
            sentMoldCommentText.text = sentMoldCommentDialogue[2];
        }
        else if ( ( playerRecipe[1] - correctRecipe[1] == 3 ) || ( playerRecipe[1] - correctRecipe[1] == -3 ) || ( playerRecipe[2] - correctRecipe[2] == 3 ) || ( playerRecipe[2] - correctRecipe[2] == -3 ))
        {
            coins += 1;
            coinsEarned.text = "+1 Coins";
            
            sentMoldCommentText.text = "";
            sentMoldCommentText.text = sentMoldCommentDialogue[1];
        }
        else
        {
            coins += 0;
            coinsEarned.text = "+0 Coins";
            
            sentMoldCommentText.text = "";
            sentMoldCommentText.text = sentMoldCommentDialogue[0];
        }

        RefreshCoinsCounters();
        
        /*// Perfect Recipe
        if (playerRecipe[0] == correctRecipe[0] && playerRecipe[1] == correctRecipe[1] && playerRecipe[2] == correctRecipe[2])
        {
            coins += 5;
            RefreshCoinsCounters();
            ScoreCanvas.SetActive(true);
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

            // Partial Score
            if ( ( isPositive && ( playerRecipe[1] > 0 ) ) || ( !isPositive && ( playerRecipe[2] > 0 ) ) )
            {
                coins += 0;
                // text for having completely failed 
            }
            else if ( ( playerRecipe[0] - correctRecipe[0] == 1 ) || ( playerRecipe[0] - correctRecipe[0] == -1 ) )
            {
                coins += 3;
                // text for almost getting it
            }
            else if ( ( playerRecipe[0] - correctRecipe[0] == 2 ) || ( playerRecipe[0] - correctRecipe[0] == -2 ) )
            {
                coins += 2;
                // text for getting near
            }
            else if ( ( playerRecipe[0] - correctRecipe[0] == 3 ) || ( playerRecipe[0] - correctRecipe[0] == -3 ) )
            {
                coins += 1;
                // text for barely getting it
            }

            RefreshCoinsCounters();
        }*/

        if (coins >= 30)
        {
            NewDayCanvas.SetActive(false);
            winCanvas.SetActive(true);

            youWinText.text = youWinDialogue;
        }
    }

    public void YouDie()
    {
        NewDayCanvas.SetActive(false);
        requestCanvas.SetActive(false);
        loseCanvas.SetActive(true);

        if (typeOfDeath == 0)
        {
            youLoseText.text = "";
            youLoseText.text = youLoseDialogue[0];
        }
        else if (typeOfDeath == 1)
        {
            youLoseText.text = "";
            youLoseText.text = youLoseDialogue[1];
        }
        else if (typeOfDeath == 2)
        {
            youLoseText.text = "";
            youLoseText.text = youLoseDialogue[2];
        }
        else if (typeOfDeath == 3)
        {
            youLoseText.text = "";
            youLoseText.text = youLoseDialogue[3];
        }
        else if (typeOfDeath == 4)
        {
            youLoseText.text = "";
            youLoseText.text = youLoseDialogue[4];

            gameOverText.text = "GAME OVER...?";
        }
        else if (typeOfDeath == 5)
        {
            youLoseText.text = "";
            youLoseText.text = youLoseDialogue[5];
        }
    }

    public void PlaySoundEffect(string testSubjectStringForSound)
    {
        switch (testSubjectStringForSound)
        {
            case "rat":
                ratAudio.Play(0);
                break;
            case "cat":
                catAudio.Play(0);
                break;
            case "dog":
                dogAudio.Play(0);
                break;
            case "you":
                youAudio.Play(0);
                break;
            default:
                Debug.Log("Missing sound name inside button");
                break;
        }
    }

    // costo ingredienti
    // acqua = 0
    // verdura e carne = 1
    // si possono comprare solo i topi!
    // topi = 2

    public int GetCost(int ingredientIndex)
    {
        switch (ingredientIndex)
            {
                // acqua
                case 0:
                    return 0;
                // carne
                case 1:
                    return 1;
                // verdura
                case 2:
                    return 1;
                default:
                    return 1;
            }
    }

    public void NextClient()
    {
        Debug.Log("NextClienting");

        GenerateRecipe();
        //GenerateRecipeDescription();
        RecipeRequest();

        // reset muffa
        for (int j=0; j<numIngredients; j++)
        {
            playerRecipe[j] = 0;
            playerRecipeText[j].SetText("0");
        }
        cupFilledText.text = "Total: 0/10";
        cupFilledTotal = 0;

        //Reset beaker
        beakerColor[2] = 0;
        beakerColor[0] = 0;
        beakerColor[1] = 0;
        beakerColor[3] = 1;
        beakerImage.GetComponent<Image>().color = beakerColor;
        beakerImage.sprite = beakerSprites[0];

        passNightButton.interactable = false;

        ScoreCanvas.SetActive(false);
        requestCanvas.SetActive(true);
        IngredientMixingCanvas.SetActive(true);
    }

    public void RefreshCoinsCounters()
    {
        for (int j=0; j<coinsCounter.Length; j++)
        {
            coinsCounter[j].SetText("Coins: " + coins.ToString());
        }
    }

    public void BuyRat()
    {
        ratBought = true;
        
        buyRatButton.interactable = false;
        coins -= ratCost;
        RefreshCoinsCounters();

        ratTestButton.interactable = true;
        ratImage.sprite = originalRatImage;
        ratOption.SetActive(true);

        catOption.SetActive(false);
        dogOption.SetActive(false);
        youOption.SetActive(false);
    }

    private void ChangeBeakerColor()
    {
        //Prendi quantità ingredienti e trasformali in colore, poi applicalo
        beakerColor[2] = playerRecipe[0];
        beakerColor[0] = playerRecipe[1];
        beakerColor[1] = playerRecipe[2];
        beakerColor[3] = 1;

        beakerImage.GetComponent<Image>().color = beakerColor;
    }

    public void ChangeBeakerSprite()
    {
        beakerImage.sprite = beakerSprites[cupFilledTotal];
    }

    public void BackToMoldActions()
    {
        testCommentsCanvas.SetActive(false);
        //requestCanvas.SetActive(true);
        //NewDayCanvas.SetActive(true);
    }

    public void Update()
    {
        if (coins == 0 && cupFilledTotal < 10)
        {
            requestCanvas.SetActive(false);
            IngredientMixingCanvas.SetActive(false);
            loseCanvas.SetActive(true);

            typeOfDeath = 0;
            youLoseText.text = "";
            youLoseText.text = youLoseDialogue[0];
        }
    }
}