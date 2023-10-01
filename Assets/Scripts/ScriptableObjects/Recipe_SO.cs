using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Recipe_SO : ScriptableObject
{
    public List<KitchenObject_SO> kitchenObject_SOList;
    public string recipeName;
}
