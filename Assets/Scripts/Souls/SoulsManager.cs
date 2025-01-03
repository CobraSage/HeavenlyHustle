using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoulsManager : MonoBehaviour
{
    public static SoulsManager Instance { get; private set; }
    public GameObject soulPrefab; 
    public float spawnInterval = 2.0f; 
    private int maxSouls = 100; 
    public List<SoulsInformation> allSouls = new List<SoulsInformation>();
    public List<SoulsInformation> EngagedSouls = new List<SoulsInformation>();
    public List<SoulsInformation> FreeSouls = new List<SoulsInformation>();

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

    private void Start()
    {
        UpdateSoulLists();
        StartCoroutine(SpawnSouls());
    }

    public void UpdateSoulLists()
    {
        EngagedSouls.Clear();
        FreeSouls.Clear();

        foreach (SoulsInformation soul in allSouls)
        {
            if (soul.isEngaged)
            {
                EngagedSouls.Add(soul);
            }
            else
            {
                FreeSouls.Add(soul);
            }
        }
    }

    public void UpdateSoulLists(SoulsInformation soul)
    {
        if (soul.isEngaged)
        {
            if (!EngagedSouls.Contains(soul)) EngagedSouls.Add(soul);
            FreeSouls.Remove(soul);
        }
        else
        {
            if (!FreeSouls.Contains(soul)) FreeSouls.Add(soul);
            EngagedSouls.Remove(soul);
        }
    }

    private void SendSoulToBuilding(SoulsInformation soul, int buildingIndex)
    {
        BuildingsInformation building = BuildingsManager.Instance.buildingsList.Find(b => b.buildingIndex == buildingIndex);
        building.currentCapacity += 1;
        soul.EngageSoul();
        BuildingsManager.Instance.UpdateFreeBuildingsList();
        if (soul != null)
        {
            soul.gameObject.GetComponent<SoulsMovement>().StartMovement(false, buildingIndex);
        }
    }

    private IEnumerator SpawnSouls()
    {
        while (true)
        {
            if((maxSouls > 0 && HeavenManager.Instance.heavenCurrentCapacity < HeavenManager.Instance.heavenTotalCapacity))
            {
                Vector3 spawnPosition = BuildingsManager.Instance.spawnTile.transform.position;
                spawnPosition.y = 0;

                GameObject newSoul = Instantiate(soulPrefab, spawnPosition, Quaternion.identity);
                newSoul.transform.parent = this.transform;

                SoulsInformation soulInfo = newSoul.GetComponent<SoulsInformation>();
                allSouls.Add(soulInfo);
                HeavenManager.Instance.heavenCurrentCapacity++;

                AssignSoulToBuilding(soulInfo);

                maxSouls--;
                UpdateSoulLists();

                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(spawnInterval); 
            }
        }
    }

    public void AssignSoulToBuilding(SoulsInformation soulInfo)
    {
        if (BuildingsManager.Instance.freeBuildingsList.Count > 0)
        {
            BuildingsInformation freeBuilding;
            if (BuildingsManager.Instance.freeBuildingsList.Count > 1)
            {
                freeBuilding = BuildingsManager.Instance.freeBuildingsList[Random.Range(0, BuildingsManager.Instance.freeBuildingsList.Count)];
                SendSoulToBuilding(soulInfo, freeBuilding.buildingIndex);
            }
            else if (BuildingsManager.Instance.freeBuildingsList.Count <= 1)
            {
                if (!soulInfo.gameObject.GetComponent<SoulsMovement>().isAtStartingTile && soulInfo.gameObject.GetComponent<SoulsMovement>().currentBuildingIndex == BuildingsManager.Instance.freeBuildingsList[0].buildingIndex)
                {
                    StartCoroutine(soulInfo.gameObject.GetComponent<SoulsMovement>().SoulsFreeRoaming());
                }
                else
                {
                    freeBuilding = BuildingsManager.Instance.freeBuildingsList[0];
                    SendSoulToBuilding(soulInfo, freeBuilding.buildingIndex);
                }
            }
            else
            {
                UtilityScript.LogError("Weird Condition in Souls Manager's AssignSoulToBuilding function!");
                return;
            }
        }
        else
        {
            StartCoroutine(soulInfo.gameObject.GetComponent<SoulsMovement>().SoulsFreeRoaming());
        }
    }

    public float ReturnAverageHappiness()
    {
        if (allSouls.Count == 0)
            return 0f;

        float totalHappiness = 0f;
        foreach (SoulsInformation soul in allSouls)
        {
            totalHappiness += soul.happinessLevel;
        }
        return totalHappiness / allSouls.Count;
    }
}