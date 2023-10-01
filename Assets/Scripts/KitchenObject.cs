using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObject_SO kitchenObject_SO;

    private IKitchenObjectPlayer kitchenObjectParent;

    public KitchenObject_SO GetKitchenObject_SO()
    {
        return kitchenObject_SO;
    }

    public void SetKitchenObjectParent(IKitchenObjectPlayer kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HaskitchenObject())
        {
            Debug.LogError("Kitchen Object Parent already has a kitchenObject");
        }

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectPlayer GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }


    public static KitchenObject SpawnKitchenObject(KitchenObject_SO kitchenObject_SO, IKitchenObjectPlayer kitchenObjectPlayer) {

        Transform kitchenObjectTransform = Instantiate(kitchenObject_SO.prefab);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectPlayer);

        return kitchenObject;
    }
}
