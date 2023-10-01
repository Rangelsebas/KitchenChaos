using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;

    [SerializeField] private FryingRecipe_SO[] fryingRecipe_SOArray;
    [SerializeField] private BurningRecipe_SO[] burningRecipe_SOArray;

    private float fryingTimer = 0f;
    private float burningTimer = 0f;
    private FryingRecipe_SO fryingRecipe_SO;
    private BurningRecipe_SO burningRecipe_SO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HaskitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipe_SO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipe_SO.fryingTimerMax)
                    {
                        //Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipe_SO.output, this);


                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipe_SO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObject_SO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipe_SO.burningTimerMax
                    });

                    if (burningTimer > burningRecipe_SO.burningTimerMax)
                    {
                        //Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipe_SO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            //There is no kitchen object here
            if (player.HaskitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObject_SO()))
                {
                    //Player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipe_SO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObject_SO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipe_SO.fryingTimerMax
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

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObject_SO inputKitchenObjectSO)
    {
        FryingRecipe_SO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObject_SO GetOutputForInput(KitchenObject_SO inputKitchenObjectSO)
    {
        FryingRecipe_SO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipe_SO GetFryingRecipeSOWithInput(KitchenObject_SO inputKitchenObjectSO)
    {
        foreach (FryingRecipe_SO fryingRecipeSO in fryingRecipe_SOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private BurningRecipe_SO GetBurningRecipeSOWithInput(KitchenObject_SO inputKitchenObjectSO)
    {
        foreach (BurningRecipe_SO burningRecipe_SO in burningRecipe_SOArray)
        {
            if (burningRecipe_SO.input == inputKitchenObjectSO)
            {
                return burningRecipe_SO;
            }
        }

        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
