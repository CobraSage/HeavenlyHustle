using UnityEngine;

public class HandleBuildingClick : MonoBehaviour
{
    public PurchaseManager purchaseManager;
    private void OnEnable()
    {
        BuildingClickHandler.OnBuildingClicked += HandleClickOnBuilding;
    }

    private void OnDisable()
    {
        BuildingClickHandler.OnBuildingClicked -= HandleClickOnBuilding;
    }

    private void HandleClickOnBuilding(BuildingClickHandler building)
    {
        purchaseManager.OnUpgradeBuildingButtonClicked(building.gameObject.GetComponent<BuildingsInformation>().buildingIndex);
    }
}
