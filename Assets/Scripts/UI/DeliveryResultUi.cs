using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUi : MonoBehaviour
{

    private const string POPUP = "PopUp";


    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite failedSprite;
    [SerializeField] private Sprite successSprite;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Start()
    {
        DeliveryManeger.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManeger.Instance.OnRecipeFailed += Deliverymanager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void Deliverymanager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        //Debug.Log($"Success! Color: {successColor}, Alpha: {successColor.a}");
        //Debug.Log($"Success Sprite: {successSprite?.name ?? "NULL"}");

        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);

        // 强制设置颜色（包括 Alpha）
        backgroundImage.color = new Color(successColor.r, successColor.g, successColor.b, 1f);
        iconImage.sprite = successSprite;
        iconImage.color = Color.white; // 确保 icon 不是透明的

        messageText.text = "DELIVERY\nSUCCESS";

        //Debug.Log($"Background Image Color after set: {backgroundImage.color}");
    }
}
