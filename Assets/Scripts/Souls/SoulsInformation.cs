using UnityEngine;
using System.Collections;

public class SoulsInformation : MonoBehaviour
{
    public int happinessLevel; 
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
        AdjustHappinessLevel(2);
        SoulsManager.Instance.UpdateSoulLists(this);
        SoulsManager.Instance.AssignSoulToBuilding(this);
        BuildingsManager.Instance.buildingsList[GetComponent<SoulsMovement>().currentBuildingIndex].currentCapacity -= 1;
        BuildingsManager.Instance.UpdateFreeBuildingsList();
    }

    public void AdjustHappinessLevel(int amount)
    {
        happinessLevel = Mathf.Clamp(happinessLevel + amount, 0, 10);
    }
}
