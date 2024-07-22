using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    [field: Header("Building Purchase")]
    [SerializeField] private Button confirmPurchase;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button cancelPurchase;
    [SerializeField] private List<Button> buildingPurchaseButtons = new List<Button>();
    [SerializeField] private GameObject confirmationUI;
    private int currentBuildingIndex = 99;

    [field: Header("Building Upgrade")]
    [SerializeField] private TextMeshProUGUI buildingLevelText;
    [SerializeField] private TextMeshProUGUI buildingLevelPrice;
    [SerializeField] private Button buildingLevelButton;
    
    [SerializeField] private TextMeshProUGUI buildingCapacityLevelText;
    [SerializeField] private TextMeshProUGUI buildingCapacityLevelPrice;
    [SerializeField] private Button buildingCapacityLevelButton;
    
    [SerializeField] private TextMeshProUGUI buildingTimeLevelText;
    [SerializeField] private TextMeshProUGUI buildingTimeLevelPrice;
    [SerializeField] private Button buildingTimeLevelButton;

    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button closeUpgradeUIButton;

    private enum TypesOfPurchase{ Level, Capacity, Time}

    private int currentBuildingLevelPrice = 0;
    private int currentBuildingCapacityLevelPrice = 0;
    private int currentBuildingTimeLevelPrice = 0;

    private void Start()
    {
        #region Building Unlocking
        foreach (var button in buildingPurchaseButtons)
        {
            button.onClick.AddListener(() => OnPurchaseBuildingButtonClicked(button.GetComponentInParent<BuildingsInformation>().buildingIndex));
        }
        confirmPurchase.onClick.AddListener(() => ConfirmBuildingPurchase());
        cancelPurchase.onClick.AddListener(() => CancelBuildingPurchase());
        confirmationUI.SetActive(false);
        #endregion

        #region Building Upgrade
        buildingLevelPrice = buildingLevelButton.GetComponentInChildren<TextMeshProUGUI>();
        buildingCapacityLevelPrice = buildingCapacityLevelButton.GetComponentInChildren<TextMeshProUGUI>();
        buildingTimeLevelPrice = buildingTimeLevelButton.GetComponentInChildren<TextMeshProUGUI>();
        closeUpgradeUIButton.onClick.AddListener(() => OnCloseUpgradeUIClicked());
        #endregion
    }

    #region Building Unlocking
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
        confirmationText.text = "Are you sure you want to buy the " + BuildingsManager.Instance.buildingsList[currentBuildingIndex].buildingName + " for " + BuildingsManager.Instance.buildingsList[currentBuildingIndex].buildingUnlockPrice + " Happiness Points?";
    }

    private void ConfirmBuildingPurchase()
    {
        int buildingPrice = BuildingsManager.Instance.buildingsList[currentBuildingIndex].buildingUnlockPrice;
        if (CurrencyManager.Instance.GetHappinessPointsAmount() >= buildingPrice)
        {
            CurrencyManager.Instance.AdjustHappinessPoints(-buildingPrice);
            BuildingsManager.Instance.UnlockBuilding(currentBuildingIndex);
            CancelBuildingPurchase();
        }
        else
        {
            UtilityScript.LogError("Not Enough Money to buy Building with index " +  currentBuildingIndex);
            return;
        }
    }

    private void CancelBuildingPurchase()
    {
        Time.timeScale = 1;
        confirmationUI.SetActive(false);
    }
    #endregion

    #region Building Upgrades
    public void OnUpgradeBuildingButtonClicked(int index)
    {
        currentBuildingIndex = index;
        Time.timeScale = 0;
        UpdateUpgradeUIValues();
        upgradeUI.SetActive(true);
    }

    private void UpdateUpgradeUIValues()
    {
        BuildingsInformation currentBuilding = BuildingsManager.Instance.buildingsList[currentBuildingIndex];

        buildingNameText.text = currentBuilding.buildingName + " Upgrade Menu";
        buildingLevelText.text = currentBuilding.buildingLevel >= 5 ? "Building Level MAX" : "Building Level x" + currentBuilding.buildingLevel.ToString();
        currentBuildingLevelPrice = Mathf.RoundToInt(currentBuilding.buildingLevelBasePrice * (Mathf.Pow(1.5f, currentBuilding.buildingLevel - 1)));
        buildingLevelPrice.text = currentBuilding.buildingLevel >= 5 ? "Max" : (currentBuildingLevelPrice).ToString();

        buildingCapacityLevelText.text = currentBuilding.totalCapacityLevel >= 5 ? "Building Capacity MAX" : "Building Capacity x" + currentBuilding.totalCapacityLevel.ToString();
        currentBuildingCapacityLevelPrice = Mathf.RoundToInt(currentBuilding.buildingCapacityBasePrice * (Mathf.Pow(1.5f, currentBuilding.totalCapacityLevel - 1)));
        Debug.Log((currentBuilding.totalCapacityLevel >= 5) + " Hello " + currentBuilding.totalCapacityLevel);
        buildingCapacityLevelPrice.text = currentBuilding.totalCapacityLevel >= 5 ? "Max" : (currentBuildingCapacityLevelPrice).ToString();

        buildingTimeLevelText.text = currentBuilding.timeForEntertainmentLevel >= 5 ? "Engagement Timer MAX" : "Engagement Timer x" + currentBuilding.timeForEntertainmentLevel.ToString(); ;
        currentBuildingTimeLevelPrice = Mathf.RoundToInt(currentBuilding.buildingTimeBasePrice * (Mathf.Pow(1.5f, currentBuilding.timeForEntertainmentLevel - 1)));
        buildingTimeLevelPrice.text = currentBuilding.timeForEntertainmentLevel >= 5 ? "Max" : (currentBuildingTimeLevelPrice).ToString();

        CheckAffordability(currentBuilding);
        AssignEventsToButton();
    }

    private void CheckAffordability(BuildingsInformation currentBuilding)
    {
        int currentHappinessPointsInHand = CurrencyManager.Instance.GetHappinessPointsAmount();

        buildingLevelButton.interactable = (currentBuilding.buildingLevel < 5 && currentHappinessPointsInHand >= currentBuildingLevelPrice) ? true : false;
        buildingCapacityLevelButton.interactable = (currentBuilding.totalCapacityLevel < 5 && currentHappinessPointsInHand >= currentBuildingCapacityLevelPrice) ? true : false;
        buildingTimeLevelButton.interactable = (currentBuilding.timeForEntertainmentLevel < 5 && currentHappinessPointsInHand >= currentBuildingTimeLevelPrice) ? true : false;

    }

    private void AssignEventsToButton()
    {
        buildingLevelButton.onClick.RemoveAllListeners();
        buildingCapacityLevelButton.onClick.RemoveAllListeners();
        buildingTimeLevelButton.onClick.RemoveAllListeners();

        buildingLevelButton.onClick.AddListener(() => OnPurchaseUpgradeClicked(TypesOfPurchase.Level));
        buildingCapacityLevelButton.onClick.AddListener(() => OnPurchaseUpgradeClicked(TypesOfPurchase.Capacity));
        buildingTimeLevelButton.onClick.AddListener(() => OnPurchaseUpgradeClicked(TypesOfPurchase.Time));
    }

    private void OnPurchaseUpgradeClicked(TypesOfPurchase typesOfPurchase)
    {
        int currentHappinessPointsInHand = CurrencyManager.Instance.GetHappinessPointsAmount();
        switch (typesOfPurchase)
        {
            case TypesOfPurchase.Level:
                if(currentHappinessPointsInHand < currentBuildingLevelPrice)
                {
                    UtilityScript.LogError("Insufficient Amount to Upgrade Building with Index " + currentBuildingIndex);
                    return;
                }
                CurrencyManager.Instance.AdjustHappinessPoints(-currentBuildingLevelPrice);
                BuildingsManager.Instance.UpgradeBuildingLevel(currentBuildingIndex);
                break;

            case TypesOfPurchase.Capacity:
                if (currentHappinessPointsInHand < currentBuildingCapacityLevelPrice)
                {
                    UtilityScript.LogError("Insufficient Amount to Upgrade Building Capacity with Index " + currentBuildingIndex);
                    return;
                }
                CurrencyManager.Instance.AdjustHappinessPoints(-currentBuildingCapacityLevelPrice);
                BuildingsManager.Instance.UpgradeTotalCapacityLevel(currentBuildingIndex);
                break;

            case TypesOfPurchase.Time:
                if (currentHappinessPointsInHand < currentBuildingTimeLevelPrice)
                {
                    UtilityScript.LogError("Insufficient Amount to Upgrade Building Time with Index " + currentBuildingIndex);
                    return;
                }
                CurrencyManager.Instance.AdjustHappinessPoints(-currentBuildingTimeLevelPrice);
                BuildingsManager.Instance.UpgradeTimeForEntertainment(currentBuildingIndex);
                break;

            default:
                Debug.Log("Unknown type of Purchase");
                break;
        }
        UpdateUpgradeUIValues();
    }

    private void OnCloseUpgradeUIClicked()
    {
        currentBuildingIndex = 99;
        buildingLevelButton.onClick.RemoveAllListeners();
        buildingCapacityLevelButton.onClick.RemoveAllListeners();
        buildingTimeLevelButton.onClick.RemoveAllListeners();
        Time.timeScale = 1.0f;
        upgradeUI.SetActive(false);
    }    
    #endregion
}