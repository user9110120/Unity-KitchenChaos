using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArry;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangedEvendArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }else
        {
            Hide();
        }

    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArry)
        {
            visualGameObject.SetActive(true);
        }
        
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArry)
        {
            visualGameObject.SetActive(false);
        }
    }
}
