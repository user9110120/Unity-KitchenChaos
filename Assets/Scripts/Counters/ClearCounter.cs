using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter 
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    



    public override void Interact(Player player)
    {
        //Pick up and drop
        if (!HasKitchenObject())
        {
            //There is no kitchenobject here
            if (player.HasKitchenObject())
            {
                //Player is carring something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {
                //Player not carring anything
            }
        } else
        {
            //There is a kitchenobject here
            if (player.HasKitchenObject())
            {
                //Player is carring something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holdng a plate
                    if (plateKitchenObject.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestorySelf();
                    }
                }else
                {
                    //Player is not carrying Plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //Counter is holding a plate
                        if (plateKitchenObject.TryAddIngrediant(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestorySelf();
                        }
                    }
                }
            }
            else
            {
                //Player is not carring anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    
}
