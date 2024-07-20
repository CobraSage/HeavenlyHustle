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

    [field: Header("Switching Stuff")]
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera sideGameCamera;
    [SerializeField] private Button switchButton;
    [SerializeField] private GameObject sideGameObject;
    [SerializeField] private Image fadeImage; 
    [SerializeField] private float fadeDuration = 1f;

    private PersonInformation currentPerson;
    private bool isSideGameActive = false;

    private float cooldownTime;
    private bool isCooldownActive = false;

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
        switchButton.onClick.AddListener(() => SwitchNow());
        fadeImage.gameObject.SetActive(false);
        switchButton.interactable = true;
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

    private void SwitchGames()
    {
        if(!isSideGameActive)
        {
            mainCamera.transform.tag = "Untagged";
            mainCamera.gameObject.SetActive(false);

            sideGameCamera.transform.tag = "MainCamera";
            sideGameCamera.gameObject.SetActive(true);

            PeopleManager.Instance.SelectAndActivateRandomScene();
            cursorManager.checkForPeople = true;
            sideGameObject.SetActive(true);
            isSideGameActive = true;
        }
        else
        {
            sideGameCamera.transform.tag = "Untagged";
            sideGameCamera.gameObject.SetActive(false);
            
            mainCamera.transform.tag = "MainCamera";
            mainCamera.gameObject.SetActive(true);

            PeopleManager.Instance.OnCloseSideGame();
            cursorManager.StopCursorChange();
            sideGameObject.SetActive(false);
            isSideGameActive = false;

            StartCoroutine(CooldownTimer());
        }
    }

    private IEnumerator FadeTransition()
    {
        fadeImage.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(1f));

        SwitchGames();

        yield return StartCoroutine(Fade(0f));

        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;

        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            float blend = Mathf.Clamp01(t / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, blend);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
    private void SwitchNow()
    { 
        StartCoroutine(FadeTransition());
    }
    private IEnumerator CooldownTimer()
    {
        switchButton.interactable = false;
        isCooldownActive = true;
        cooldownTime = HeavenManager.Instance.sideGameCooldownTime;
        TextMeshProUGUI buttonText = switchButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        while (cooldownTime > 0)
        {
            int minutes = Mathf.FloorToInt(cooldownTime / 60);
            int seconds = Mathf.FloorToInt(cooldownTime % 60);
            buttonText.text = $"{minutes:00}:{seconds:00}";
            yield return new WaitForSeconds(1f);
            cooldownTime -= 1f;
        }

        switchButton.interactable = true;
        isCooldownActive = false;
        buttonText.text = "Switch";
    }
}
