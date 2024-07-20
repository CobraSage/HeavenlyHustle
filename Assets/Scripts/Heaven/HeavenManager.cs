using UnityEngine;

public class HeavenManager : MonoBehaviour
{
    public static HeavenManager Instance { get; private set; }

    public int heavenTotalCapacity = 10;
    public int heavenTotalCapacityLevel = 1;
    public int heavenCurrentCapacity = 0;
    public float sideGameCooldownTime = 5 * 60f;
    public float sideGameCooldownReductionLevel = 1;
    public float soulsMultiplier = 1f;
    public int soulsMultiplierLevel = 1;
    public float happinessPointsMultiplier = 1f;
    public int happinessPointsMultiplierLevel = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadInitialValues();
    }
    public void UpgradeHeavenTotalCapacityLevel()
    {
        if (heavenTotalCapacityLevel >= 5)
        {
            UtilityScript.LogError("heavenTotalCapacityLevel is at max level?!");
            return;
        }
        heavenTotalCapacityLevel++;
        heavenTotalCapacity += 10;
        // Saving&Loading
    }

    public void UpgradeSoulsMultiplierLevel()
    {
        if (soulsMultiplierLevel >= 5)
        {
            UtilityScript.LogError("soulsMultiplierLevel is at max level?!");
            return;
        }
        soulsMultiplierLevel++;
        soulsMultiplier += 0.5f;
        // Saving&Loading
    }

    public void UpgradeHappinessPointsMultiplierLevel()
    {
        if (happinessPointsMultiplierLevel >= 5)
        {
            UtilityScript.LogError("happinessPointsMultiplierLevel is at max level?!");
            return;
        }
        happinessPointsMultiplierLevel++;
        happinessPointsMultiplier += 0.5f;
        // Saving&Loading
    }

    private void LoadInitialValues()
    {
        PurchasesData loadedValues = PurchasesDataManager.Instance.ReturnLoadedPurchasesData();
        heavenTotalCapacityLevel = loadedValues.HeavenTotalCapacityLevel;
        sideGameCooldownReductionLevel = loadedValues.SideGameCooldownReductionLevel;
        soulsMultiplierLevel = loadedValues.SoulsMultiplierLevel;
        happinessPointsMultiplierLevel = loadedValues.HappinessPointsMultiplierLevel;

        heavenTotalCapacity = 0 + (5 * heavenTotalCapacityLevel);
        heavenCurrentCapacity = 0;
        sideGameCooldownTime = (5.5f - (0.5f * sideGameCooldownReductionLevel)) * 60f;
        soulsMultiplier = 0.5f + (0.5f * soulsMultiplierLevel);
        happinessPointsMultiplier = 0.5f + (0.5f * happinessPointsMultiplierLevel);
    }
}
