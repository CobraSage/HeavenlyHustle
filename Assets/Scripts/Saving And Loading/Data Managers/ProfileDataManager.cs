using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ProfileDataManager : MonoBehaviour
{
    public static ProfileDataManager Instance { get; private set; }

    private ProfileData profileData;
    private const string FileName = "PfnRsOsnF1l3.json";

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

        LoadProfileData();
    }

    //private void Start()
    //{
    //    ExampleUsage();
    //}

    private void LoadProfileData()
    {
        if (PersistentDataManager.Instance.SaveFileExists(FileName))
        {
            profileData = PersistentDataManager.Instance.LoadFromFile<ProfileData>(FileName);
        }
        else
        {
            profileData = new ProfileData();
            SaveProfileData(); 
        }
    }

    private void SaveProfileData()
    {
        PersistentDataManager.Instance.SaveToFile(FileName, profileData);
    }

    public void UpdateAndSaveSoulsAmount(int value)
    {
        profileData.soulsAmount = value;
        SaveProfileData();
    }

    public void UpdateAndSaveHappinessPointsAmount(int value)
    {
        profileData.happinessPointsAmount = value;
        SaveProfileData();
    }

    public void UpdateAndSaveHappinessMeter(float value)
    {
        profileData.happinessMeter = value;
        SaveProfileData();
    }

    public void UpdateAndSaveGodSatisfactionMeter(float value)
    {
        profileData.godSatisfactionMeter = value;
        SaveProfileData();
    }

    public void UpdateAndSaveEarthEvilMeter(float value)
    {
        profileData.earthEvilMeter = value;
        SaveProfileData();
    }

    public ProfileData ReturnLoadedProfileData()
    {
        return PersistentDataManager.Instance.LoadFromFile<ProfileData>(FileName);
    }

    public void ExampleUsage()
    {
        UpdateAndSaveSoulsAmount(69);
        UpdateAndSaveEarthEvilMeter(7.5f);
        UpdateAndSaveHappinessMeter(5.4f);

        ProfileData loadedData = PersistentDataManager.Instance.LoadFromFile<ProfileData>(FileName);
        Debug.Log($"SoulsAmount: {loadedData.soulsAmount}");
        Debug.Log($"EarthEvilMeter: {loadedData.earthEvilMeter}");
        Debug.Log($"HappinessMeter: {loadedData.happinessMeter}");
    }
}
