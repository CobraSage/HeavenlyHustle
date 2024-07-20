using UnityEngine;
using System.Collections;

public class SoulsInformation : MonoBehaviour
{
    public float happinessLevel; 
    public bool isEngaged = false;

    public void EngageSoul()
    {
        if (!isEngaged)
        {
            isEngaged = true;
            SoulsManager.Instance.UpdateSoulLists(this);
        }
    }

    public void StartEngagementTimer(float time)
    {
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(EngagementTimer(time));
    }

    private IEnumerator EngagementTimer(float time)
    {
        yield return new WaitForSeconds(time);
        OnEngagementComplete();
    }

    private void OnEngagementComplete()
    {
        GetComponent<MeshRenderer>().enabled = true;
        isEngaged = false;

        BuildingsInformation currentBuilding = BuildingsManager.Instance.buildingsList[GetComponent<SoulsMovement>().currentBuildingIndex];

        float currentBuildingLevel = currentBuilding.buildingLevel;
        float happinessLevelToAdjust = (0.1f * currentBuildingLevel) * (Random.Range(0f, 1f) < 0.75f ? 1 : -1);
        AdjustHappinessLevel(happinessLevelToAdjust);
        
        SoulsManager.Instance.UpdateSoulLists(this);
        SoulsManager.Instance.AssignSoulToBuilding(this);
        currentBuilding.currentCapacity -= 1;
        BuildingsManager.Instance.UpdateFreeBuildingsList();
    }

    public void AdjustHappinessLevel(float amount)
    {
        happinessLevel = Mathf.Clamp(happinessLevel + amount, 0, 10);
        MetersManager.Instance.UpdateHappinessMeter();
    }
}
