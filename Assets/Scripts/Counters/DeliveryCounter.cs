using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) return;

        if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;

        if (plateKitchenObject == null) return;

        if (DeliveryManeger.Instance == null)
        {
            Debug.LogError("DeliveryManager.Instance is null!");
            return;
        }

        // 交付订单
        DeliveryManeger.Instance.DeliverRecipe(plateKitchenObject);

        // 销毁盘子（只执行一次）
        player.GetKitchenObject().DestorySelf();
    }


}
