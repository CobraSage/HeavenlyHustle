using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager Instance { get; private set; }
    public GameObject spawnTile;

    public List<BuildingsInformation> buildingsList = new List<BuildingsInformation>();
    public List<BuildingsInformation> freeBuildingsList = new List<BuildingsInformation>();
    public List<BuildingsInformation> unlockedBuildingsList = new List<BuildingsInformation>();

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
        UpdateFreeBuildingsList();
        UpdateUnlockedBuildingsList();
    }

    public void UnlockBuilding(int buildingIndex)
    {
        BuildingsInformation building = buildingsList.Find(b => b.buildingIndex == buildingIndex);
        if (building != null && !building.isUnlocked)
        {
            building.OnUnlock();
            PurchasesDataManager.Instance.UpdateAndSaveBuildingsUnlocked(buildingIndex, true);
        }
        else if (building == null)
        {
            UtilityScript.LogError("Building " + building.name + " is null!");
            return;
        }
        else if(building.isUnlocked)
        {
            UtilityScript.LogError("Building " + building.name + " is already unlocked!");
            return; 
        }
    }

    public void UpgradeBuildingLevel(int buildingIndex)
    {
        BuildingsInformation building = buildingsList.Find(b => b.buildingIndex == buildingIndex);
        if (building.buildingLevel >= 5)
        {
            UtilityScript.LogError("Building " + building.name + " is at max level?!");
            return;
        }
        if (building != null && building.isUnlocked)
        {
            building.buildingLevel++;
            PurchasesDataManager.Instance.UpdateAndSaveBuildingsLevel(buildingIndex, building.buildingLevel);
        }
        else if (building == null)
        {
            UtilityScript.LogError("Building " + building.name + " is null!");
            return;
        }
        else if (!building.isUnlocked)
        {
            UtilityScript.LogError("Building " + building.name + " is locked so you cannot upgrade it my friend!");
            return;
        }
    }

    public void UpgradeTotalCapacityLevel(int buildingIndex)
    {
        BuildingsInformation building = buildingsList.Find(b => b.buildingIndex == buildingIndex);
        if (building.totalCapacityLevel >= 5)
        {
            UtilityScript.LogError("Building " + building.name + "'s capacity is at max level?!");
            return;
        }
        if (building != null && building.isUnlocked)
        {
            building.totalCapacityLevel++;
            building.totalCapacity += 5;
            PurchasesDataManager.Instance.UpdateAndSaveBuildingsCapacity(buildingIndex, building.totalCapacityLevel);
        }
        else if (building == null)
        {
            UtilityScript.LogError("Building " + building.name + " is null!");
            return;
        }
        else if (!building.isUnlocked)
        {
            UtilityScript.LogError("Building " + building.name + " is locked so you cannot upgrade it my friend!");
            return;
        }
    }

    public void UpgradeTimeForEntertainment(int buildingIndex)
    {
        BuildingsInformation building = buildingsList.Find(b => b.buildingIndex == buildingIndex);
        if (building.timeForEntertainmentLevel >= 5)
        {
            UtilityScript.LogError("Building " + building.name + "'s Time For Entertainment is at max level?!");
            return;
        }
        if (building != null && building.isUnlocked)
        {
            building.timeForEntertainmentLevel++;
            building.timeForEntertainment -= 0.5f;
            PurchasesDataManager.Instance.UpdateAndSaveBuildingsTimeLevel(buildingIndex, building.timeForEntertainmentLevel);
        }
        else if (building == null)
        {
            UtilityScript.LogError("Building " + building.name + " is null!");
            return;
        }
        else if (!building.isUnlocked)
        {
            UtilityScript.LogError("Building " + building.name + " is locked so you cannot upgrade it my friend!");
            return;
        }
    }

    public void UpdateFreeBuildingsList()
    {
        freeBuildingsList = new List<BuildingsInformation>();
        freeBuildingsList = buildingsList.Where(b => b.isUnlocked && b.currentCapacity < b.totalCapacity).ToList();
    }
    public void UpdateUnlockedBuildingsList()
    {
        unlockedBuildingsList = new List<BuildingsInformation>();
        unlockedBuildingsList = buildingsList.Where(b => b.isUnlocked).ToList();
    }
}