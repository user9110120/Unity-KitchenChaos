using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{

    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;


    private void Start()
    {
        DeliveryManeger.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawned;
        DeliveryManeger.Instance.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
    }

    private void DeliveryManager_OnRecipeSpawned (object sender, System.EventArgs e)
    {
        updateVisual();
    }

    private void DeliveryManager_OnRecipeComplete(object sender, System.EventArgs e)
    {
        updateVisual();
    }
    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void updateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManeger.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }


}
