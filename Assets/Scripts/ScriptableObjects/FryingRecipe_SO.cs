using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FryingRecipe_SO : ScriptableObject
{
    public KitchenObject_SO input;
    public KitchenObject_SO output;
    public float fryingTimerMax;
}
