using UnityEngine;
using System.Collections.Generic;

public class SoulsManager : MonoBehaviour
{
    public static SoulsManager Instance { get; private set; }

    public List<SoulsInformation> allSouls = new List<SoulsInformation>();
    public List<SoulsInformation> EngagedSouls = new List<SoulsInformation>();
    public List<SoulsInformation> FreeSouls = new List<SoulsInformation>();
    public List<BuildingsInformation> buildings = new List<BuildingsInformation>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateSoulLists();
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

    public void SendSoulToBuilding(SoulsInformation soul, int buildingIndex)
    {
        BuildingsInformation building = buildings.Find(b => b.buildingIndex == buildingIndex);
        if (building != null)
        {
            //PathNode startNode = soul.GetComponent<PathNode>(); // Assuming each soul starts at a specific path node
            //PathNode targetNode = building.pathToBuilding[building.pathToBuilding.Count - 1]; // Last node in the path to building
            //SoulsMovement soulMovement = soul.GetComponent<SoulsMovement>();
            //soulMovement.FindPath(startNode, targetNode);
        }
    }
}