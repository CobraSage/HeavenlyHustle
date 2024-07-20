using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PurchasesDataManager : MonoBehaviour
{
    public static PurchasesDataManager Instance { get; private set; }

    private PurchasesData purchasesData;
    private const string FileName = "PuRc4a63s.json";

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

        LoadPurchasesData();
    }

    //private void Start()
    //{
    //    ExampleUsage();
    //}

    private void LoadPurchasesData()
    {
        if (PersistentDataManager.Instance.SaveFileExists(FileName))
        {
            purchasesData = PersistentDataManager.Instance.LoadFromFile<PurchasesData>(FileName);
        }
        else
        {
            purchasesData = new PurchasesData();
            SavePurchasesData(); 
        }
    }

    private void SavePurchasesData()
    {
        PersistentDataManager.Instance.SaveToFile(FileName, purchasesData);
    }

    public void UpdateAndSaveBuildingsUnlocked(int index, bool value)
    {
        purchasesData.BuildingsUnlocked[index] = value;
        SavePurchasesData();
    }

    public void UpdateAndSaveBuildingsLevel(int index, int value)
    {
        purchasesData.BuildingsLevel[index] = value;
        SavePurchasesData();
    }

    public void UpdateAndSaveBuildingsCapacity(int index, int value)
    {
        purchasesData.BuildingsCapacity[index] = value;
        SavePurchasesData();
    }

    public void UpdateAndSaveBuildingsTimeLevel(int index, int value)
    {
        purchasesData.BuildingsTimeLevel[index] = value;
        SavePurchasesData();
    }

    public void UpdateAndSaveHeavenTotalCapacityLevel(int value)
    {
        purchasesData.HeavenTotalCapacityLevel = value;
        SavePurchasesData();
    }

    public void UpdateAndSaveSideGameCooldownReductionLevel(int value)
    {
        purchasesData.SideGameCooldownReductionLevel = value;
        SavePurchasesData();
    }

    public void UpdateAndSaveSoulsMultiplierLevel(int value)
    {
        purchasesData.SoulsMultiplierLevel = value;
        SavePurchasesData();
    }

    public void UpdateAndSaveHappinessPointsMultiplierLevel(int value)
    {
        purchasesData.HappinessPointsMultiplierLevel = value;
        SavePurchasesData();
    }

    public PurchasesData ReturnLoadedPurchasesData()
    {
        return PersistentDataManager.Instance.LoadFromFile<PurchasesData>(FileName);
    }

    public void ExampleUsage()
    {
        UpdateAndSaveBuildingsUnlocked(1, true);
        UpdateAndSaveBuildingsLevel(2, 3);
        UpdateAndSaveHeavenTotalCapacityLevel(5);

        PurchasesData loadedData = PersistentDataManager.Instance.LoadFromFile<PurchasesData>(FileName);
        Debug.Log($"Building 2 Level: {loadedData.BuildingsLevel[2]}");
        Debug.Log($"Heaven Total Capacity Level: {loadedData.HeavenTotalCapacityLevel}");
        Debug.Log($"Building 1 Unlocked: {loadedData.BuildingsUnlocked[1]}");
    }
}
