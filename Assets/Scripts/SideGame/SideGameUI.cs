using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideGameUI : MonoBehaviour
{
    [field: Header("Person Details UI")]
    [SerializeField] private TextMeshProUGUI personName;
    [SerializeField] private TextMeshProUGUI personAge;
    [SerializeField] private TextMeshProUGUI personDoB;
    [SerializeField] private TextMeshProUGUI personDoD;
    [SerializeField] List<TextMeshProUGUI> personalityList = new List<TextMeshProUGUI>();
    [SerializeField] Button BanishButton;
    [SerializeField] Button NVMButton;
    [SerializeField] GameObject personDetailsUIObject;


    [field: Header("Switching Stuff")]
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera sideGameCamera;
    [SerializeField] private Button switchButton;
    [SerializeField] private GameObject sideGameObject;
    [SerializeField] private Image fadeImage; 
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject mainSceneButton;

    [field: Header("Game Over UI")]
    [SerializeField] GameObject gameOverUIObject;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI conclusionText;
    [SerializeField] private TextMeshProUGUI soulsEarned;
    [SerializeField] private Button moveOnButton;
    private List<string> losingStrings = new List<string>
    {
        "Oops! You sent an innocent soul away and let the villain stay. Are you working for the dark side?",
        "You exiled the wrong person! Now chaos reigns. Are you sure you’re not a demon in disguise?",
        "The wrong soul was banished, and evil still roams free. Are you a secret agent of darkness?",
        "You let the wicked one slip through your fingers! Is there a devilish side to you?",
        "An innocent soul is gone, and the evil one remains. Are you playing for the other team?",
        "You banished the wrong soul, and now the world suffers. Are you sure you’re not a devil’s advocate?",
        "The most evil person is still here, thanks to you. Are you secretly plotting with the underworld?",
        "You sent the wrong soul packing, leaving evil behind. Are you a hidden harbinger of doom?",
        "The innocent are gone, and the wicked thrive. Are you a covert agent of chaos?",
        "You exiled the wrong person, and now darkness prevails. Are you secretly a minion of the devil?"
    };
    private List<string> winningStrings = new List<string>
    {
        "Great job! The evil has been banished, and Earth is safer thanks to you!",
        "You did it! The villain is gone, and the world is a better place because of your actions!",
        "Success! The wicked one is no more, and peace prevails. Well done!",
        "Congratulations! You’ve rid the world of evil, making it a brighter place!",
        "Victory! The dark force has been banished, and Earth is grateful for your bravery!",
        "Well done! The evil has been vanquished, and harmony is restored thanks to you!",
        "Fantastic! The villain is banished, and the world rejoices in your triumph!",
        "Bravo! You’ve sent the evil packing, and Earth is a bit more heavenly now!",
        "Excellent work! The wicked one is gone, and the world is a safer place because of you!",
        "Hooray! The evil has been banished, and Earth is a better place thanks to your efforts!"
    };

    [SerializeField] GameObject uiObject;

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
        gameOverUIObject.SetActive(false);
        personDetailsUIObject.SetActive(false);
        ClearAllText();
    }

    private void Start()
    {
        BanishButton.onClick.AddListener(() => OnButtonsClicked(true));
        NVMButton.onClick.AddListener(() => OnButtonsClicked(false));
        switchButton.onClick.AddListener(() => SwitchNow());
        moveOnButton.onClick.AddListener(() => OnMoveOnButtonClicked());
        fadeImage.gameObject.SetActive(false);
        switchButton.interactable = true;
    }

    #region Person Details
    public void UpdateValues(PersonInformation personInfo)
    {
        currentPerson = personInfo;
        personName.text = personInfo.Name;
        personAge.text = personInfo.Age.ToString();
        personDoB.text = personInfo.DateOfBirth;
        personDoD.text = personInfo.DateOfDeath;
        PopulatePersonalityList(personInfo);
        gameOverUIObject.SetActive(false);
        personDetailsUIObject.SetActive(true);
        uiObject.SetActive(true);
        cursorManager.StopCursorChange();
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
            personalityList[i].text = "- " + combinedDeeds[i];
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
                TriggerGameOver(true);
            }
            else
            {
                TriggerGameOver(false);
            }
        }
        else
        {
            CloseAndResetUI();
            cursorManager.checkForPeople = true;
        }
    }
    #endregion

    #region Game Over
    private void TriggerGameOver(bool hasWon)
    {
        gameOverUIObject.SetActive(true);
        personDetailsUIObject.SetActive(false);
        if (hasWon)
        {
            int soulsEarnedValue = 1 * HeavenManager.Instance.soulsMultiplierLevel;
            resultText.text = "Evil Soul Banished!";
            conclusionText.text = winningStrings[Random.Range(0,winningStrings.Count)];
            soulsEarned.text = "Souls Earned: " + soulsEarnedValue.ToString();
            CurrencyManager.Instance.AdjustSouls(soulsEarnedValue);
            MetersManager.Instance.UpdateEvilMeter(-1);
        }
        else
        {
            resultText.text = "Innocent Soul Banished...";
            conclusionText.text = losingStrings[Random.Range(0, losingStrings.Count)];
            soulsEarned.text = "Souls Earned: 0";
            MetersManager.Instance.UpdateEvilMeter(1);
        }
    }

    private void OnMoveOnButtonClicked()
    {
        gameOverUIObject.SetActive(false);
        personDetailsUIObject.SetActive(false);
        CloseAndResetUI();
        SwitchNow();
    }
    #endregion

    #region Switching Games
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
            switchButton.gameObject.SetActive(false);
            mainSceneButton.SetActive(false);
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
            switchButton.gameObject.SetActive(true);
            mainSceneButton.SetActive(true);

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
    #endregion

    #region Cooldown
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
        buttonText.text = "Play";
    }
    #endregion
}
