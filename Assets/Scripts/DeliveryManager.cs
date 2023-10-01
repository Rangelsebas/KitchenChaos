using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set;}

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeList_SO recipeList_SO;

    private List<Recipe_SO> waitingRecipe_SOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int succesfulRecipesAmount;

    private void Awake()
    {
        Instance = this;

        waitingRecipe_SOList = new List<Recipe_SO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipe_SOList.Count < waitingRecipesMax)
            {
                Recipe_SO waitingRecipeSO = recipeList_SO.recipe_SOList[UnityEngine.Random.Range(0, recipeList_SO.recipe_SOList.Count)];

                waitingRecipe_SOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipe_SOList.Count; i++)
        {
            Recipe_SO waitingRecipeSO = waitingRecipe_SOList[i];

            if (waitingRecipeSO.kitchenObject_SOList.Count == plateKitchenObject.GetKitchenObject_SOList().Count)
            {
                //Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObject_SO recipeKitcheObjectSO in waitingRecipeSO.kitchenObject_SOList)
                {
                    //Cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObject_SO plateKitchenObjectSO in plateKitchenObject.GetKitchenObject_SOList())
                    {
                        //Cycling through all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitcheObjectSO)
                        {
                            //Ingredients matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        //This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    //Payer delivered the correct recipe!
                    succesfulRecipesAmount++;

                    Debug.Log("Payer delivered the correct recipe!");

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        //No matches found!
        //Player did not delivered the correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<Recipe_SO> GetWaitingRecipeSOList()
    {
        return waitingRecipe_SOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return succesfulRecipesAmount;
    }

}
