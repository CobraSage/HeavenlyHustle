using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class SoulsMovement : MonoBehaviour
{
    [field: Header("Movement")]
    public int currentBuildingIndex = 0;
    public float speed = 5f;
    public bool isFreeRoaming = false;
    public bool isAtStartingTile = true;

    private List<Transform> currentPath = new List<Transform>();
    private int currentTargetHouse = 0;
    private bool currentMoveToSpawn = false;

    public void StartMovement(bool moveToSpawn = false, int targetHouseIndex = 99)
    {
        if (isFreeRoaming)
        {
            currentTargetHouse = targetHouseIndex;
            currentMoveToSpawn = moveToSpawn;
            isFreeRoaming = false;
        }
        else
        {
            PlotPathToHouse(moveToSpawn, targetHouseIndex);
        }
    }

    private void PlotPathToHouse(bool moveToSpawn = false, int targetHouseIndex = 99)
    {
        currentPath = new List<Transform>();
        if (!isAtStartingTile && !moveToSpawn && currentBuildingIndex == targetHouseIndex)
        {
            return;
        }
        if (isAtStartingTile)
        {
            currentPath.Add(BuildingsManager.Instance.spawnTile.transform);
            isAtStartingTile = false;
        }
        if (!moveToSpawn)
        {
            if (BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == currentBuildingIndex).turningTile != null)
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
            currentPath.Clear();
            currentPath.Add(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == currentBuildingIndex).turningTile.transform);
            currentPath.Add(BuildingsManager.Instance.spawnTile.transform);
            isAtStartingTile = true;
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
        currentPath.Clear();
        if (!isFreeRoaming)
        {
            GetComponent<SoulsInformation>().StartEngagementTimer(BuildingsManager.Instance.buildingsList.FirstOrDefault(b => b.buildingIndex == currentBuildingIndex).timeForEntertainment);
        }
    }

    public IEnumerator SoulsFreeRoaming()
    {
        isFreeRoaming = true;
        int targetHouseIndex = currentBuildingIndex;
        GetComponent<SoulsInformation>().AdjustHappinessLevel(-0.05f);

        if(BuildingsManager.Instance.unlockedBuildingsList.Count > 1)
        {
            while (targetHouseIndex == currentBuildingIndex)
            {
                targetHouseIndex = Random.Range(0, BuildingsManager.Instance.unlockedBuildingsList.Count);
            }
            PlotPathToHouse(false, targetHouseIndex);
        }
        else if(!isAtStartingTile && targetHouseIndex == currentBuildingIndex && BuildingsManager.Instance.unlockedBuildingsList.Count <= 1)
        {
            PlotPathToHouse(true);
        }
        else
        {
            targetHouseIndex = currentBuildingIndex;
            PlotPathToHouse(false, targetHouseIndex);
        }

        currentBuildingIndex = targetHouseIndex;

        yield return new WaitUntil(() => currentPath.Count == 0);
        yield return new WaitForSeconds(2);

        isFreeRoaming = false;
        SoulsManager.Instance.AssignSoulToBuilding(this.gameObject.GetComponent<SoulsInformation>());
        yield return null;
    }

}
