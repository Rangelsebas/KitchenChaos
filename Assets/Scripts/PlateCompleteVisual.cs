using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [System.Serializable]
    public struct KitchenObject_SO_GameObject
    {
        public KitchenObject_SO kitchenObject_SO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObject_SO_GameObject> kitchenObject_SO_GameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObject_SO_GameObject kitchenObject_SO_GameObject in kitchenObject_SO_GameObjectList)
        {
            kitchenObject_SO_GameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObject_SO_GameObject kitchenObject_SO_GameObject in kitchenObject_SO_GameObjectList)
        {
            if (kitchenObject_SO_GameObject.kitchenObject_SO == e.kitchenObject_SO)
            {
                kitchenObject_SO_GameObject.gameObject.SetActive(true);
            }
        }
    }
}
