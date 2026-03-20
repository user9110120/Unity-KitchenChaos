using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTempalte;


    private void Awake()
    {
        iconTempalte.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateKitchenObject.OnIgredientAdded += PlateKitchenObject_OnIgredientAdded;
    }

    private void PlateKitchenObject_OnIgredientAdded(object sender, PlateKitchenObject.OnIgredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTempalte) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTempalte, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjctSO(kitchenObjectSO);
        }
    }
}
