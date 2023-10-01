using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObject_SO kitchenObject_SO;

    public override void Interact(Player player)
    {
        if (!player.HaskitchenObject()) {
            //Player is not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObject_SO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
