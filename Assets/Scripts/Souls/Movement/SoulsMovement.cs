using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoulsMovement : MonoBehaviour
{
    [field: Header("Movement")]
    public int currentBuildingIndex = 0;
    public bool isEngaged = false;
    public float speed = 5f;

    private List<Transform> currentPath = new List<Transform>();
    private bool justSpawned = true;

    public void PlotPathToHouse(bool moveToSpawn = false, int targetHouseIndex = 99)
    {
        currentPath = new List<Transform>();
        if (!moveToSpawn && currentBuildingIndex == targetHouseIndex)
        {
            return;
        }
        if (justSpawned)
        {
            currentPath.Add(BuildingsManager.Instance.spawnTile.transform);
            justSpawned = false;
        }
        if (!moveToSpawn)
        {
            if(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == currentBuildingIndex).turningTile != null)
            {
                currentPath.Add(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == currentBuildingIndex).turningTile.transform);
            }
            else
            {
                currentPath.Add(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == currentBuildingIndex).targetTile.transform);
            }
            if (BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == targetHouseIndex).turningTile != null)
            {
                currentPath.Add(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == targetHouseIndex).turningTile.transform);
            }
            currentPath.Add(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == targetHouseIndex).targetTile.transform);
            currentBuildingIndex = targetHouseIndex;
        }
        else if (moveToSpawn)
        {
            currentPath.Add(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == currentBuildingIndex).turningTile.transform);
            currentPath.Add(BuildingsManager.Instance.spawnTile.transform);
        }
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        foreach (Transform target in currentPath)
        {
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
