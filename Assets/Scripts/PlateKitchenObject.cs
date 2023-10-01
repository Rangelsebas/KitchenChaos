using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs
    {
        public KitchenObject_SO kitchenObject_SO;
    }
    [SerializeField] private List<KitchenObject_SO> validKitchenObject_SOList;

    private List<KitchenObject_SO> kitchenObject_SOList;

    private void Awake()
    {
        kitchenObject_SOList = new List<KitchenObject_SO>();
    }

    public bool TryAddIngredient(KitchenObject_SO kitchenObject_SO)
    {
        if (!validKitchenObject_SOList.Contains(kitchenObject_SO))
        {
            //Not a valid ingredient
            return false;
        }
        if (kitchenObject_SOList.Contains(kitchenObject_SO))
        {
            //Already has this type
            return false;
        }
        else
        {
            kitchenObject_SOList.Add(kitchenObject_SO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObject_SO = kitchenObject_SO
            });
            return true;
        }
        
    }

    public List<KitchenObject_SO> GetKitchenObject_SOList()
    {
        return kitchenObject_SOList;
    }
}
