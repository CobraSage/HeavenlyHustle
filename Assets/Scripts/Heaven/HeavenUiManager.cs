using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeavenUiManager : MonoBehaviour
{
    private enum UIType { Meters, HeavenUpgrade, SoulsReckoning}
    private enum purchaseType { heavenCapacity, soulGameCooldown, soulsMultiplier, pointsMultiplier}


    [field: Header("General Stuff")]
    [SerializeField] private GameObject heavenUiObject;
    [SerializeField] private Button openUiButton;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private TextMeshProUGUI leftArrowText;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private TextMeshProUGUI rightArrowText;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject mainCanvasObject;

    [field: Header("Meters UI Stuff")]
    [SerializeField] private GameObject metersUIObject;
    [SerializeField] private Slider happinessMeter;
    [SerializeField] private Slider evilOnEarthMeter;
    [SerializeField] private Slider godSatisfactionMeter;
    [SerializeField] private TextMeshProUGUI probabilityText;

    [field: Header("Heaven Upgrades UI Stuff")]
    [SerializeField] private GameObject heavenUpgradesUIObject;
    [SerializeField] private List<TextMeshProUGUI> upgradesTitleTexts;
    [SerializeField] private List<Button> purchaseButtons;

    [field: Header("Souls Reckoning UI Stuff")]
    [SerializeField] private GameObject soulsReckoningUIObject;
    [SerializeField] private TextMeshProUGUI currentSoulsText;
    [SerializeField] private TextMeshProUGUI activeMultiplierText;
    [SerializeField] private Button playButton;

    private void Start()
    {
        heavenUiObject.SetActive(false);
        metersUIObject.SetActive(false);
        heavenUpgradesUIObject.SetActive(false);
        soulsReckoningUIObject.SetActive(false);
        openUiButton.onClick.AddListener(() => OpenUI());
        closeButton.onClick.AddListener(() => CloseUI());
        mainCanvasObject.SetActive(true);
        playButton.onClick.AddListener(() => { mainCanvasObject.SetActive(true); mainCanvasObject.GetComponentInChildren<Button>().gameObject.SetActive(false);  heavenUiObject.SetActive(false); });
    }

    private void OpenUI()
    {
        SwitchUI(UIType.HeavenUpgrade);
        heavenUiObject.SetActive(true);
        mainCanvasObject.SetActive(false);
    }
    private void CloseUI()
    {
        heavenUiObject.SetActive(false);
        mainCanvasObject.SetActive(true);
    }

    private void SwitchUI(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.HeavenUpgrade:
                HeavenUpgradeUiInitialization();
                break;

            case UIType.Meters:
                MetersUiInitialization();
                break;

            case UIType.SoulsReckoning:
                SoulsReckoningUiInitialization();
                break;

            default:
                Debug.Log("Unknown type of UI Type");
                break;
        }
    }

    #region Heaven Upgrade
    private void HeavenUpgradeUiInitialization()
    {
        metersUIObject.SetActive(false);
        soulsReckoningUIObject.SetActive(false);
        heavenUpgradesUIObject.SetActive(true);

        leftArrowButton.onClick.RemoveAllListeners();
        leftArrowButton.onClick.AddListener(() => SwitchUI(UIType.Meters));
        leftArrowText.text = "METERS";

        rightArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.AddListener(() => SwitchUI(UIType.SoulsReckoning));
        rightArrowText.text = "SOULS";

        titleText.text = "HEAVEN UPGRADES";

        UpdateUpgradeUiElements();
    }

    private void OnPurchaseUpgradeButtonClicked(purchaseType purch)
    {
        int currentSoulPoints = CurrencyManager.Instance.GetSoulsAmount();
        switch (purch)
        {
            case purchaseType.heavenCapacity:
                int currentHeavenCapacityLevel = HeavenManager.Instance.heavenTotalCapacityLevel;
                int currentHeavenCapacityPrice = HeavenManager.Instance.heavenTotalCapacityBasePrice * (int)Mathf.Pow(2, currentHeavenCapacityLevel - 1);
                if(currentSoulPoints < currentHeavenCapacityPrice)
                {
                    UtilityScript.LogError("Insufficient Balance to Upgrade Heaven Capacity!");
                    return;
                }
                CurrencyManager.Instance.AdjustSouls(-currentHeavenCapacityPrice);
                HeavenManager.Instance.UpgradeHeavenTotalCapacityLevel();
                break;

            case purchaseType.soulGameCooldown:
                int currentSoulGameCooldownLevel = HeavenManager.Instance.sideGameCooldownReductionLevel;
                int currentSoulGameCooldownPrice = HeavenManager.Instance.sideGameCooldownReductionBasePrice * (int)Mathf.Pow(2, currentSoulGameCooldownLevel - 1);
                if (currentSoulPoints < currentSoulGameCooldownPrice)
                {
                    UtilityScript.LogError("Insufficient Balance to Upgrade Soul Game Cooldwon!");
                    return;
                }
                CurrencyManager.Instance.AdjustSouls(-currentSoulGameCooldownPrice);
                HeavenManager.Instance.UpgradeSideGameCooldownLevel();
                break;

            case purchaseType.soulsMultiplier:
                int currentSoulsMultiplierLevel = HeavenManager.Instance.soulsMultiplierLevel;
                int currentSoulsMultiplierPrice = HeavenManager.Instance.soulsMultiplierBasePrice * (int)Mathf.Pow(2, currentSoulsMultiplierLevel - 1);
                if (currentSoulPoints < currentSoulsMultiplierPrice)
                {
                    UtilityScript.LogError("Insufficient Balance to Upgrade Souls Multiplier!");
                    return;
                }
                CurrencyManager.Instance.AdjustSouls(-currentSoulsMultiplierPrice);
                HeavenManager.Instance.UpgradeSoulsMultiplierLevel();
                break;

            case purchaseType.pointsMultiplier:
                int currentPointsMultiplierLevel = HeavenManager.Instance.happinessPointsMultiplierLevel;
                int currentPointsMultiplierPrice = HeavenManager.Instance.happinessPointsBaseMultiplierPrice * (int)Mathf.Pow(2, currentPointsMultiplierLevel - 1);
                if (currentSoulPoints < currentPointsMultiplierPrice)
                {
                    UtilityScript.LogError("Insufficient Balance to Upgrade Happiness Points Multiplier!");
                    return;
                }
                CurrencyManager.Instance.AdjustSouls(-currentPointsMultiplierPrice);
                HeavenManager.Instance.UpgradeHappinessPointsMultiplierLevel();
                break;

            default:
                Debug.Log("Unknown type of Purchase!!");
                break;
        }
        UpdateUpgradeUiElements();
    }
    
    private void UpdateUpgradeUiElements()
    {
        int currentSoulPoints = CurrencyManager.Instance.GetSoulsAmount();
        Debug.Log("currentSoulPoints = " + currentSoulPoints);

        int currentHeavenCapacityLevel = HeavenManager.Instance.heavenTotalCapacityLevel;
        int currentHeavenCapacityPrice = HeavenManager.Instance.heavenTotalCapacityBasePrice * (int)Mathf.Pow(2, currentHeavenCapacityLevel - 1);
        Debug.Log("currentHeavenCapacityPrice = " + currentHeavenCapacityPrice);
        upgradesTitleTexts[0].text = "Heaven Capacity " + (currentHeavenCapacityLevel >=5 ? "MAX" : "X" + currentHeavenCapacityLevel.ToString());
        purchaseButtons[0].interactable = (currentSoulPoints < currentHeavenCapacityPrice || currentHeavenCapacityLevel >= 5) ? false : true;
        purchaseButtons[0].onClick.RemoveAllListeners();
        purchaseButtons[0].onClick.AddListener(() => OnPurchaseUpgradeButtonClicked(purchaseType.heavenCapacity));
        purchaseButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = currentHeavenCapacityLevel.ToString();

        int currentSoulGameCooldownLevel = HeavenManager.Instance.sideGameCooldownReductionLevel;
        int currentSoulGameCooldownPrice = HeavenManager.Instance.sideGameCooldownReductionBasePrice * (int)Mathf.Pow(2, currentSoulGameCooldownLevel - 1);
        Debug.Log("currentSoulGameCooldownPrice = " + currentSoulGameCooldownPrice);
        upgradesTitleTexts[1].text = "Soul Game Cooldown " + (currentSoulGameCooldownLevel >= 5 ? "MAX" : "X" + currentSoulGameCooldownLevel.ToString());
        purchaseButtons[1].interactable = (currentSoulPoints < currentSoulGameCooldownPrice || currentSoulGameCooldownLevel >= 5) ? false : true;
        purchaseButtons[1].onClick.RemoveAllListeners();
        purchaseButtons[1].onClick.AddListener(() => OnPurchaseUpgradeButtonClicked(purchaseType.soulGameCooldown));
        purchaseButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = currentSoulGameCooldownLevel.ToString();

        int currentSoulsMultiplierLevel = HeavenManager.Instance.soulsMultiplierLevel;
        int currentSoulsMultiplierPrice = HeavenManager.Instance.soulsMultiplierBasePrice * (int)Mathf.Pow(2, currentSoulsMultiplierLevel - 1);
        Debug.Log("currentSoulsMultiplierPrice = " + currentSoulsMultiplierPrice);
        upgradesTitleTexts[2].text = "Souls Multiplier " + (currentSoulsMultiplierLevel >= 5 ? "MAX" : "X" + currentSoulsMultiplierLevel.ToString());
        purchaseButtons[2].interactable = (currentSoulPoints < currentSoulsMultiplierPrice || currentSoulsMultiplierLevel >= 5) ? false : true;
        purchaseButtons[2].onClick.RemoveAllListeners();
        purchaseButtons[2].onClick.AddListener(() => OnPurchaseUpgradeButtonClicked(purchaseType.soulsMultiplier));
        purchaseButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = currentSoulsMultiplierLevel.ToString();

        int currentPointsMultiplierLevel = HeavenManager.Instance.happinessPointsMultiplierLevel;
        int currentPointsMultiplierPrice = HeavenManager.Instance.happinessPointsBaseMultiplierPrice * (int)Mathf.Pow(2, currentPointsMultiplierLevel - 1);
        Debug.Log("Point Mult Price = " +  currentPointsMultiplierPrice);
        upgradesTitleTexts[3].text = "Points Multiplier " + (currentPointsMultiplierLevel >= 5 ? "MAX" : "X" + currentPointsMultiplierLevel.ToString());
        purchaseButtons[3].interactable = (currentSoulPoints < currentPointsMultiplierPrice || currentPointsMultiplierLevel >= 5) ? false : true;
        purchaseButtons[3].onClick.RemoveAllListeners();
        purchaseButtons[3].onClick.AddListener(() => OnPurchaseUpgradeButtonClicked(purchaseType.pointsMultiplier));
        purchaseButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = currentPointsMultiplierLevel.ToString();
    }
    #endregion

    #region Souls Reckoning
    private void SoulsReckoningUiInitialization()
    {
        metersUIObject.SetActive(false);
        heavenUpgradesUIObject.SetActive(false);
        soulsReckoningUIObject.SetActive(true);

        leftArrowButton.onClick.RemoveAllListeners();
        leftArrowButton.onClick.AddListener(() => SwitchUI(UIType.HeavenUpgrade));
        leftArrowText.text = "UPGRADE";

        rightArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.AddListener(() => SwitchUI(UIType.Meters));
        rightArrowText.text = "METERS";

        titleText.text = "SOUL'S RECKONING";

        UpdateSoulsReckoningUIElements();
    }

    private void UpdateSoulsReckoningUIElements()
    {
        currentSoulsText.text = "CURRENT SOULS: " + CurrencyManager.Instance.GetSoulsAmount().ToString();
        activeMultiplierText.text = "ACTIVE MULTIPLIER: X" + HeavenManager.Instance.soulsMultiplierLevel.ToString(); 
    }
    #endregion

    #region Meters
    private void MetersUiInitialization()
    {
        heavenUpgradesUIObject.SetActive(false);
        soulsReckoningUIObject.SetActive(false);
        metersUIObject.SetActive(true);

        leftArrowButton.onClick.RemoveAllListeners();
        leftArrowButton.onClick.AddListener(() => SwitchUI(UIType.SoulsReckoning));
        leftArrowText.text = "SOULS";

        rightArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.AddListener(() => SwitchUI(UIType.HeavenUpgrade));
        rightArrowText.text = "UPGRADES";

        titleText.text = "METERS";

        UpdateMetersUIElements();
    }

    private void UpdateMetersUIElements()
    {
        float happinessValue = MetersManager.Instance.happinessMeter / 10f;
        float godSatisfactionValue = MetersManager.Instance.godSatisfactionMeter / 10f;
        float earthEvilValue = MetersManager.Instance.earthEvilMeter / 10f;

        happinessMeter.value = happinessValue;
        evilOnEarthMeter.value = earthEvilValue;
        godSatisfactionMeter.value = godSatisfactionValue;

        float probability = (1 - happinessValue) * 0.4f + (1 - godSatisfactionValue) * 0.4f + earthEvilValue * 0.2f;
        int probabilityOfGettingFired = Mathf.RoundToInt(probability * 100);

        probabilityText.text = "Probability Of Getting Fired: " + probabilityOfGettingFired.ToString() + "%";
    }
    #endregion
}
