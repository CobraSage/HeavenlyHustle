using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetersManager : MonoBehaviour
{
    public static MetersManager Instance { get; private set; }

    public float happinessMeter = 1.0f;
    public float godSatisfactionMeter = 5.0f;
    public float earthEvilMeter = 1.0f;

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
        LoadInitialValues();
    }

    public void UpdateEvilMeter(float value)
    {
        earthEvilMeter += value;
        earthEvilMeter = Mathf.Clamp(earthEvilMeter, 0f, 10f);
        UpdateGodSatisfactionMeter();
    }

    public void UpdateHappinessMeter()
    {
        happinessMeter = SoulsManager.Instance.ReturnAverageHappiness();
        happinessMeter = Mathf.Clamp(happinessMeter, 0f, 10f);
        UpdateGodSatisfactionMeter();
    }

    public void UpdateGodSatisfactionMeter()
    {
        float normalizedHappiness = happinessMeter / 10f;
        float normalizedEarthEvil = earthEvilMeter / 10f;

        float normalizedGodSatisfaction = normalizedHappiness * (1 - normalizedEarthEvil);

        godSatisfactionMeter = normalizedGodSatisfaction * 10f;

        godSatisfactionMeter = Mathf.Clamp(godSatisfactionMeter, 0f, 10f);

        if (godSatisfactionMeter < 1.0f)
        {
            //GameOverSequence.Instance.OnGameOverSequenceInitiated();
        }
    }

    private void LoadInitialValues()
    {
        ProfileData loadedData = ProfileDataManager.Instance.ReturnLoadedProfileData();
        happinessMeter = loadedData.happinessMeter;
        godSatisfactionMeter = loadedData.godSatisfactionMeter;
        earthEvilMeter = loadedData.earthEvilMeter;
    }
}
