using UnityEngine;
using System.Collections.Generic;

public class PeopleManager : MonoBehaviour
{
    public static PeopleManager Instance { get; private set; }

    public List<string> GoodDeeds = new List<string>
    {
        "Helps people in need",
        "Donates to charity",
        "Volunteers at shelters",
        "Rescues animal",
        "Promotes Afforestation",
        "Mentors students",
        "Properly disposes litter",
        "Cooks Meals for the homeless",
        "Donates blood",
        "Visits nursing homes",
        "Organizes community clean-ups",
        "Protects endangered species",
        "Pays for people's meals",
        "Runs in charity marathons",
        "Donates clothes to the needy",
        "Helps friends when needed",
        "Supports local businesses",
        "Sends care packages to soldiers",
        "Participates in charity events",
        "Compliments People",
        "Helps old people cross the road",
        "Donates books to library",
        "Recycles waste",
        "Hosts multiple fundraisers",
        "Ready to save someone in danger"
    };

    public List<string> BadDeeds = new List<string>
    {
        "Constant Liar",
        "Part Time Thief",
        "Big Time Cheater",
        "Loves hurting people's feelings",
        "Ignores people in need",
        "Spread multiple rumors",
        "Breaks promises",
        "Encourages Public Littering",
        "Generally Rude to Strangers",
        "Constantly skips work",
        "Enjoys damaging someone's property",
        "Part time serial killer",
        "Refuses to help people",
        "Rage Hacker in Games",
        "Attracted to Minors",
        "Gossips behind people's backs",
        "Disrespects people irrespective of age",
        "Steals credits for other people's works",
        "Loves bullying",
        "Rash Driver",
        "Loves using other people",
        "Commits Tax Evasion",
        "Damages Public Property",
        "Abuses animals",
        "Promotes Deforestation",
        "Fake Kindness Influencer"
    };

    public List<GameObject> listOfScenes = new List<GameObject>();

    public List<PersonInformation> currentPeopleList = new List<PersonInformation>();

    public PersonInformation currentMostEvilPerson = null;

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
        foreach (GameObject go in listOfScenes)
        {
            go.SetActive(false);
        }
    }

    public void SelectAndActivateRandomScene()
    {
        int selectedScene = Random.Range(0, listOfScenes.Count);
        PersonInformation[] personComponents = listOfScenes[selectedScene].GetComponentsInChildren<PersonInformation>();
        currentPeopleList = new List<PersonInformation>(personComponents);
        foreach (PersonInformation person in currentPeopleList)
        {
            person.AssignValues();
        }
        listOfScenes[selectedScene].SetActive(true);
        currentMostEvilPerson = ReturnMostEvilPerson();
    }

    public List<string> GetRandomGoodDeeds(int count)
    {
        List<string> randomGoodDeeds = new List<string>(GoodDeeds);
        Shuffle(randomGoodDeeds);
        return randomGoodDeeds.GetRange(0, Mathf.Min(count, randomGoodDeeds.Count));
    }

    public List<string> GetRandomBadDeeds(int count)
    {
        List<string> randomBadDeeds = new List<string>(BadDeeds);
        Shuffle(randomBadDeeds);
        return randomBadDeeds.GetRange(0, Mathf.Min(count, randomBadDeeds.Count));
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public PersonInformation ReturnMostEvilPerson()
    {
        if (currentPeopleList == null || currentPeopleList.Count == 0)
        {
            return null;
        }

        PersonInformation mostEvilPerson = currentPeopleList[0];

        foreach (PersonInformation person in currentPeopleList)
        {
            if (person.EvilValue > mostEvilPerson.EvilValue)
            {
                mostEvilPerson = person;
            }
        }

        return mostEvilPerson;
    }

    public void OnCloseSideGame()
    {
        foreach (GameObject go in listOfScenes)
        {
            go.SetActive(false);
        }
        foreach (PersonInformation person in currentPeopleList)
        {
            person.ClearValues();
        }
        currentMostEvilPerson = null;
        currentPeopleList.Clear();
    }
}
