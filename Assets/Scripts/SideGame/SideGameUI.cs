using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideGameUI : MonoBehaviour
{
    [field: Header("UI Objects")]
    [SerializeField] private TextMeshProUGUI personName;
    [SerializeField] private TextMeshProUGUI personAge;
    [SerializeField] private TextMeshProUGUI personDoB;
    [SerializeField] private TextMeshProUGUI personDoD;
    [SerializeField] List<TextMeshProUGUI> personalityList = new List<TextMeshProUGUI>();
    [SerializeField] Button BanishButton;
    [SerializeField] Button NVMButton;
    [SerializeField] GameObject uiObject;

    private PersonInformation currentPerson;

    public static SideGameUI Instance { get; private set; }

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

        uiObject.SetActive(false);
        ClearAllText();
    }

    private void Start()
    {
        BanishButton.onClick.AddListener(() => OnButtonsClicked(true));
        NVMButton.onClick.AddListener(() => OnButtonsClicked(false));
    }

    public void UpdateValues(PersonInformation personInfo)
    {
        currentPerson = personInfo;
        personName.text = personInfo.Name;
        personAge.text = personInfo.Age.ToString();
        personDoB.text = personInfo.DateOfBirth;
        personDoD.text = personInfo.DateOfDeath;
        PopulatePersonalityList(personInfo);
        uiObject.SetActive(true);
    }

    private void PopulatePersonalityList(PersonInformation personInfo)
    {
        List<string> combinedDeeds = new List<string>();
        combinedDeeds.AddRange(personInfo.GoodDeeds);
        combinedDeeds.AddRange(personInfo.BadDeeds);

        for (int i = 0; i < combinedDeeds.Count; i++)
        {
            int randomIndex = Random.Range(0, combinedDeeds.Count);
            string temp = combinedDeeds[i];
            combinedDeeds[i] = combinedDeeds[randomIndex];
            combinedDeeds[randomIndex] = temp;
        }

        for (int i = 0; i < personalityList.Count && i < combinedDeeds.Count; i++)
        {
            personalityList[i].text = combinedDeeds[i];
        }
    }
    public void ClearAllText()
    {
        personName.text = null;
        personAge.text = null;
        personDoB.text = null;
        personDoD.text = null;
        currentPerson = null;

        foreach (var text in personalityList)
        {
            text.text = null;
        }
    }

    private void CloseAndResetUI()
    {
        ClearAllText();
        uiObject.SetActive(false);
    }

    private void OnButtonsClicked(bool isBanishedButton)
    {
        if (isBanishedButton)
        {
            if(currentPerson == PeopleManager.Instance.currentMostEvilPerson)
            {
                Debug.Log("Correct Evil Person!");
            }
            else
            {
                Debug.Log("Wrong Person Chosen F");
            }
        }
        CloseAndResetUI();
    }
}
