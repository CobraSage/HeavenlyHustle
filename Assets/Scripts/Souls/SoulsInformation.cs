using UnityEngine;
using System.Collections;

public class SoulsInformation : MonoBehaviour
{
    public int happinessLevel; 
    public bool isEngaged = false;

    public void EngageSoul(float time)
    {
        if (!isEngaged)
        {
            isEngaged = true;
            StartCoroutine(EngagementTimer(time));
        }
    }

    private IEnumerator EngagementTimer(float time)
    {
        yield return new WaitForSeconds(time);
        OnEngagementComplete();
    }

    private void OnEngagementComplete()
    {
        isEngaged = false;
        AdjustHappinessLevel(2);
        SoulsManager.Instance.UpdateSoulLists(this);
    }

    private void AdjustHappinessLevel(int amount)
    {
        happinessLevel = Mathf.Clamp(happinessLevel + amount, 0, 10);
    }
}
