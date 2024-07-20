using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    [field: Header("Building Purchase")]
    [SerializeField] private Button confirmPurchase;
    [SerializeField] private Button cancelPurchase;
    [SerializeField] private List<Button> buildingPurchaseButtons = new List<Button>();
    [SerializeField] private GameObject confirmationUI;
    private int currentBuildingIndex = 99;

    private void Start()
    {
        foreach (var button in buildingPurchaseButtons)
        {
            button.onClick.AddListener(() => OnPurchaseBuildingButtonClicked(button.GetComponentInParent<BuildingsInformation>().buildingIndex));
        }
        confirmPurchase.onClick.AddListener(() => ConfirmPurchase());
        cancelPurchase.onClick.AddListener(() => CancelPurchase());
        confirmationUI.SetActive(false);
    }

    private void OnPurchaseBuildingButtonClicked(int index)
    {
        currentBuildingIndex = index;
        if (CurrencyManager.Instance.GetHappinessPointsAmount() >= BuildingsManager.Instance.buildingsList[currentBuildingIndex].buildingUnlockPrice)
        {
            confirmPurchase.interactable = true;
        }
        else
        {
            confirmPurchase.interactable = false;
        }
        Time.timeScale = 0;
        confirmationUI.SetActive(true);
    }

    private void ConfirmPurchase()
    {
        int buildingPrice = BuildingsManager.Instance.buildingsList[currentBuildingIndex].buildingUnlockPrice;
        if (CurrencyManager.Instance.GetHappinessPointsAmount() >= buildingPrice)
        {
            CurrencyManager.Instance.AdjustHappinessPoints(-buildingPrice);
            BuildingsManager.Instance.UnlockBuilding(currentBuildingIndex);
            CancelPurchase();
        }
        else
        {
            UtilityScript.LogError("Not Enough Money to buy Building with index " +  currentBuildingIndex);
            return;
        }
    }

    private void CancelPurchase()
    {
        Time.timeScale = 1;
        confirmationUI.SetActive(false);
    }
}
