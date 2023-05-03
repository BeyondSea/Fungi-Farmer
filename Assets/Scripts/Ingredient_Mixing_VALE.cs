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
    [SerializeField] public Button testButton;

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
    #endregion
    
// Start is called before the first frame update
    void Start()
    {
        GenerateRecipe();
        GenerateRecipeDescription();

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
        // Type
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
        }
        
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
    }

    public void PassNight()
    {
        // Animazione del passaggio della notte?

        // mostra altro canvas
        IngredientMixingCanvas.SetActive(false);
        NewDayCanvas.SetActive(true);
    }

    public void TestResult()
    {
        if (playerRecipe[1] > 0 && playerRecipe[2] > 0)
        {
            TestCommentText.text = failedTestDialogue;
        }

        else if (animalsLeft == 4)
        {
            animalsLeft--;

            testButton.interactable = false;
            
            //If rat is left, and recipe has meat
            //then if rat is left, and recipe has vegs
            if (playerRecipe[1] > correctRecipe[1])
            {
                TestCommentText.text = ratDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1])
            {
                TestCommentText.text = ratDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1])
            {
                TestCommentText.text = ratDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2])
            {
                TestCommentText.text = ratDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2])
            {
                TestCommentText.text = ratDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2])
            {
                TestCommentText.text = ratDialogue[5];
            }
            
        }

        else if (animalsLeft == 3)
        {
            animalsLeft--;

            testButton.interactable = false;
            
            //same, if cat
            if (playerRecipe[1] > correctRecipe[1])
            {
                TestCommentText.text = catDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1])
            {
                TestCommentText.text = catDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1])
            {
                TestCommentText.text = catDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2])
            {
                TestCommentText.text = catDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2])
            {
                TestCommentText.text = catDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2])
            {
                TestCommentText.text = catDialogue[5];
            }
        }

        else if (animalsLeft == 2)
        {
            animalsLeft--;

            testButton.interactable = false;
           
           //same, if dog
            if (playerRecipe[1] > correctRecipe[1])
            {
                TestCommentText.text = dogDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1])
            {
                TestCommentText.text = dogDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1])
            {
                TestCommentText.text = dogDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2])
            {
                TestCommentText.text = dogDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2])
            {
                TestCommentText.text = dogDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2])
            {
                TestCommentText.text = dogDialogue[5];
            }
        }

        else if (animalsLeft == 1)
        {
            animalsLeft--;

            testButton.interactable = false;

           //same, if you
            if (playerRecipe[1] > correctRecipe[1])
            {
                TestCommentText.text = youDialogue[0];
            }
            else if (playerRecipe[1] < correctRecipe[1])
            {
                TestCommentText.text = youDialogue[1];
            }
            else if (playerRecipe[1] == correctRecipe[1])
            {
                TestCommentText.text = youDialogue[2];
            }
            else if (playerRecipe[2] > correctRecipe[2])
            {
                TestCommentText.text = youDialogue[3];
            }
            else if (playerRecipe[2] < correctRecipe[2])
            {
                TestCommentText.text = youDialogue[4];
            }
            else if (playerRecipe[2] == correctRecipe[2])
            {
                TestCommentText.text = youDialogue[5];
            }
        }


        /*//show comments only if rats are available
        if (numberOfRats > 0)
        {
            // mostra altro canvas
            //NewDayCanvas.SetActive(false);
            //TestResultCanvas.SetActive(true);
            TestResultTextBox.SetActive(true);


            
            if (playerRecipe[1] > 0 && playerRecipe[2] > 0)
            {
                //Bla bla metti qualcosa io
                
                // mixata
                //TestCommentText.text += "Questa muffa è davvero strana, mi sa che ho fatto un casino.\n\n";
            }
            else if (playerRecipe[1] > playerRecipe[0] && isPositiveOrNegative = true)
            {
                //carne > acqua, ma dovrebbe essere vegetale
                if (animalsLeft == 4)
                {
                    TestCommentText.text = 
                }
            }
            else if (playerRecipe[1] > 0 && correctRecipe[1] == 0)
            {
                // velenosa ma dovrebbe essere benfica
                TestCommentText.text += "Il ratto è morto... forse devo lavorarci ancora un po'.\n\n";
            }
            else if(playerRecipe[2] > 0 && correctRecipe[2] > 0)
            {
                // benefica e dovrebbe essere benefica
                TestCommentText.text += "Il ratto ha il pelo lucido e sembra rinvigorito, bene.";
                TestCommentText.text += "Però si è anche messo a ballare il tango, il che è inaspettato.\n";
                TestCommentText.text += "Il ratto se ne va. Oramai è giunto il momento che spieghi le ali e prenda la sua strada\n\n";
                isCorrectType = true;
            }
            else if (playerRecipe[2] > 0 && correctRecipe[2] == 0)
            {
                // benefica ma dovrebbe essere velenosa
                TestCommentText.text += "Il ratto non mi sembra molto morto... *gunshot noise*\n";
                TestCommentText.text += "Come dice sempre la nonna, non lasciare mai testimoni.\n\n";
            }
            
            // commento sulla quantità di attivatore
            if (isCorrectType == true)
            {
                if (playerRecipe[1] < correctRecipe[1] || playerRecipe[2] < correctRecipe[2])
                {
                    // troppo poco
                    TestCommentText.text += "Gli effetti sembrano avvenire con parecchio ritardo.\n\n";
                }
                else if (playerRecipe[1] == correctRecipe[1] || playerRecipe[2] == correctRecipe[2])
                {
                    // giusto
                    TestCommentText.text += "Gli effetti sembrano avvenire con il tempismo giusto.\n\n";
                }
                else if (playerRecipe[1] > correctRecipe[1] || playerRecipe[2] > correctRecipe[2])
                {
                    // troppo
                    TestCommentText.text += "Gli effetti avvengono troppo velocemnete.\n\n";
                }
            }

            // commento sulla quantità di acqua
            if (playerRecipe[0] < correctRecipe[0])
            {
                // troppo poca
                TestCommentText.text += "E' una mia impressione ho la muffa sta avendo l'allucinazione di un'oasi?\n\n";
                
            }
            else if (playerRecipe[0] == correctRecipe[0])
            {
                // giusta
                TestCommentText.text += "La muffa dice che il segreto della sua skin routine è che beve ogni giorno la giusta quantità d'acqua.\n\n";
            }
            else
            {
                // troppa
                TestCommentText.text += "La muffa ha installato uno scivolo d'acqua\n\n";
            }

            // commento sulla quantità di fertilizzante
            /*
            if (playerRecipe[3] < correctRecipe[3])
            {
                // troppo poca
                TestCommentText.text += "Non credo che la muffa sia cresciuta abbastanza.\n\n";
            }
            else if (playerRecipe[3] == correctRecipe[3])
            {
                // giusta
                TestCommentText.text += "La muffa sembra delle dimensioni giuste.\n\n";
            }
            else
            {
                // troppa
                TestCommentText.text += "Per la miseria, la muffa è veramente troppa.\n\n";
            }
            

            numberOfRats--;
            numOfRatsText.text = numberOfRats.ToString();
        }*/
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

        if (animalsLeft > 0)
        {
            testButton.interactable = true;
        }
    }

    public void GiveToClient()
    {
        // mostra altro canvas
        NewDayCanvas.SetActive(false);
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
