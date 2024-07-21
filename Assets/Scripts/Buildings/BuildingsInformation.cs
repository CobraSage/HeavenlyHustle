using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingsInformation : MonoBehaviour
{
    [field: Header("Basic Building Related Properties")]
    public string buildingName;
    public int buildingIndex;
    public bool isUnlocked = false;
    public int buildingLevel = 1;
    public float timeForEntertainment = 5.0f;
    public int timeForEntertainmentLevel = 1;
    public int totalCapacity = 5;
    public int totalCapacityLevel = 1;
    public int currentCapacity = 0;
    [SerializeField] private float yOffset;
    [SerializeField] private float speedOfRising;

    [field: Header("Path Finding For Souls")]
    public GameObject turningTile;
    public GameObject targetTile;

    [field: Header("ButtonsManager")]
    public GameObject purchaseButton;
    public GameObject lockedIcon;

    [field: Header("Purchasing Stuff")]
    public int buildingUnlockPrice = 0;
    public int buildingLevelBasePrice = 100;
    public int buildingCapacityBasePrice = 250;
    public int buildingTimeBasePrice = 500;

    private float initialYPosition;
    public List<GameObject> pathToBuilding = new List<GameObject>();
    MeshRenderer mr;
    BoxCollider bc;

    private void Awake()
    {
        LoadInitialValues();
    }

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
        if (!isUnlocked)
        {
            mr.enabled = false;
            bc.enabled = false;
            foreach (var path in pathToBuilding)
            {
                path.GetComponent<PathRise>().ChangePathVisibility(false);
            }
        }
        else
        {
            foreach (GameObject path in pathToBuilding)
            {
                path.GetComponent<PathRise>().ChangePathVisibility(true);
            }
            mr.enabled = true;
            bc.enabled = true;
        }
        initialYPosition = transform.position.y;
        ChangeButtonStates();
    }

    public void OnUnlock()
    {
        isUnlocked = true;
        foreach (BuildingsInformation buildingInfo in BuildingsManager.Instance.buildingsList)
        {
            buildingInfo.ChangeButtonStates();
        }
        StartCoroutine(UnlockPaths());
    }

    private IEnumerator UnlockPaths()
    {
        foreach (var path in pathToBuilding)
        {
            yield return path.GetComponent<PathRise>().MovePlatformsUp();
        }
        MoveBuildingUp();
    }

    private void MoveBuildingUp()
    {
        Vector3 newPosition = transform.position;
        newPosition.y -= yOffset;
        transform.position = newPosition;
        StartCoroutine(MoveToInitialPosition());
        BuildingsManager.Instance.UpdateUnlockedBuildingsList();
        BuildingsManager.Instance.UpdateFreeBuildingsList();
    }

    private IEnumerator MoveToInitialPosition()
    {
        mr.enabled = true;
        bc.enabled = true;
        while (Mathf.Abs(transform.position.y - initialYPosition) > 0.01f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Mathf.MoveTowards(newPosition.y, initialYPosition, speedOfRising * Time.deltaTime);
            transform.position = newPosition;
            yield return null;
        }
    }

    public void ChangeButtonStates()
    {
        if(isUnlocked) 
        {
            lockedIcon.SetActive(false);
            purchaseButton.SetActive(false);
            return;
        }
        if (buildingIndex == 0)
        {
            return;
        }
        else if (buildingIndex == 1 && isUnlocked == false)
        {
            lockedIcon.SetActive(false);
            purchaseButton.SetActive(true);
        }
        else
        {
            if (BuildingsManager.Instance.buildingsList[buildingIndex - 1].isUnlocked == true)
            {
                lockedIcon.SetActive(false);
                purchaseButton.SetActive(true);
            }
            else
            {
                lockedIcon.SetActive(true);
                purchaseButton.SetActive(false);
            }
        }
    }

    private void LoadInitialValues()
    {
        PurchasesData loadedValues = PurchasesDataManager.Instance.ReturnLoadedPurchasesData();
        isUnlocked = loadedValues.BuildingsUnlocked[buildingIndex];
        buildingLevel = loadedValues.BuildingsLevel[buildingIndex];
        timeForEntertainmentLevel = loadedValues.BuildingsTimeLevel[buildingIndex];
        totalCapacityLevel = loadedValues.BuildingsCapacity[buildingIndex];

        timeForEntertainment = 16.0f - (1.0f * timeForEntertainmentLevel);
        totalCapacity = 0 + (5 * totalCapacityLevel);
        currentCapacity = 0;
}
}