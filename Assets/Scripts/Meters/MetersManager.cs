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
    }

    public void UpdateHappinessMeter()
    {
        happinessMeter = SoulsManager.Instance.ReturnAverageHappiness();
        if (happinessMeter > 5.0f)
        {
            UpdateGodSatisfactionMeter(0.25f);
        }
        else
        {
            UpdateGodSatisfactionMeter(-0.25f);
        }
    }

    public void UpdateGodSatisfactionMeter(float satisfaction)
    {
        godSatisfactionMeter += satisfaction;
        godSatisfactionMeter = Mathf.Clamp(godSatisfactionMeter, 0f, 10f);
        if(godSatisfactionMeter <= 3.0f)
        {
            Debug.Log("Game Over");
        }
    }

}
