using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs: EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnCut;
         
    [SerializeField] private CuttingRecipe_SO[] cutKitchenObject_SOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            //There is no kitchen object here
            if (player.HaskitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObject_SO())) {
                    //Player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipe_SO cuttingRecipe_SO = GetCuttinRecipeSOWithInput(GetKitchenObject().GetKitchenObject_SO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipe_SO.cuttingProgressMax
                    });
                }
            }
            else
            {
                //Player not carrying anything
            }
        }
        else
        {
            if (player.HaskitchenObject())
            {
                //The player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObject_SO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HaskitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObject_SO())) {
            //There is a kitchen object here
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipe_SO cuttingRecipe_SO = GetCuttinRecipeSOWithInput(GetKitchenObject().GetKitchenObject_SO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipe_SO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipe_SO.cuttingProgressMax)
            {
                KitchenObject_SO outputKitchenObject_SO = GetOutputForInput(GetKitchenObject().GetKitchenObject_SO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObject_SO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObject_SO inputKitchenObjectSO)
    {
        CuttingRecipe_SO cuttingRecipe_SO = GetCuttinRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipe_SO != null;
    }

    private KitchenObject_SO GetOutputForInput(KitchenObject_SO inputKitchenObjectSO)
    {
        CuttingRecipe_SO cuttingRecipe_SO = GetCuttinRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipe_SO != null)
        {
            return cuttingRecipe_SO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipe_SO GetCuttinRecipeSOWithInput(KitchenObject_SO inputKitchenObjectSO)
    {
        foreach (CuttingRecipe_SO cuttingRecipe_SO in cutKitchenObject_SOArray)
        {
            if (cuttingRecipe_SO.input == inputKitchenObjectSO)
            {
                return cuttingRecipe_SO;
            }
        }

        return null;
    }
}
