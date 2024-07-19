using UnityEngine;
using System.Collections.Generic;

public class SoulsManager : MonoBehaviour
{
    public static SoulsManager Instance { get; private set; }

    public List<SoulsInformation> allSouls = new List<SoulsInformation>();
    public List<SoulsInformation> EngagedSouls = new List<SoulsInformation>();
    public List<SoulsInformation> FreeSouls = new List<SoulsInformation>();

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
}
