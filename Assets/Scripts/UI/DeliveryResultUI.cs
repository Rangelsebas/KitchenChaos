using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "PopUp";
    
    [SerializeField] private Image backgroungImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnOnRecipeFailed;
        
        Hide();
    }

    private void DeliveryManager_OnOnRecipeFailed(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(POPUP);
        backgroungImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY:\nFAILED";
    }

    private void DeliveryManager_OnOnRecipeSuccess(object sender, EventArgs e)
    {
        Show();
        animator.SetTrigger(POPUP);
        backgroungImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY:\nSUCCESS";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
