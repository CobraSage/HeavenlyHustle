using UnityEngine;

public class HeavenManager : MonoBehaviour
{
    public static HeavenManager Instance { get; private set; }

    public int heavenTotalCapacity = 10;
    public int heavenTotalCapacityLevel = 1;
    public int heavenCurrentCapacity = 0;
    public float sideGameCooldownTime = 5 * 60f; 
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
}
