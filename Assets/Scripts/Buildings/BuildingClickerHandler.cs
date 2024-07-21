using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingClickHandler : MonoBehaviour
{
    public delegate void BuildingClicked(BuildingClickHandler building);
    public static event BuildingClicked OnBuildingClicked;
    public string buildingName;

    public void OnPointerClick(BaseEventData eventData)
    {
        if (GetComponent<BuildingsInformation>().isUnlocked && OnBuildingClicked != null)
        {
            OnBuildingClicked(this);
        }
    }

}

