using UnityEngine;
using System;
using System.Collections.Generic;

public class PersonInformation : MonoBehaviour
{
    public string Name;
    public int Age;
    public string DateOfBirth;
    public string DateOfDeath;
    public List<string> GoodDeeds = new List<string>();
    public List<string> BadDeeds = new List<string>();
    public float EvilValue;

    public void AssignValues()
    {
        Name = GenerateRandomName();

        Age = UnityEngine.Random.Range(25, 41);

        DateOfBirth = DateTime.Today.AddYears(-Age).ToString("dd/MM/yyyy");

        int randomYear = DateTime.Today.Year + UnityEngine.Random.Range(1, 26);
        int randomMonth = UnityEngine.Random.Range(1, 13);
        int randomDay = UnityEngine.Random.Range(1, DateTime.DaysInMonth(randomYear, randomMonth) + 1); 
        DateTime randomDateOfDeath = new DateTime(randomYear, randomMonth, randomDay);
        DateOfDeath = randomDateOfDeath.ToString("dd/MM/yyyy");

        int totalDeeds = 10;
        int numGoodDeeds = UnityEngine.Random.Range(0, totalDeeds + 1);
        int numBadDeeds = totalDeeds - numGoodDeeds;

        GoodDeeds = PeopleManager.Instance.GetRandomGoodDeeds(numGoodDeeds);
        BadDeeds = PeopleManager.Instance.GetRandomBadDeeds(numBadDeeds);

        EvilValue = (float)numBadDeeds / totalDeeds;
    }

    public void ClearValues()
    {
        Name = string.Empty;
        Age = 0;
        DateOfBirth = null;
        DateOfDeath = null;
        GoodDeeds.Clear();
        BadDeeds.Clear();
        EvilValue = 0f;
    }

    private string GenerateRandomName()
    {
        string[] firstNames = { "John", "Jane", "James", "Alice", "Robert", "Emily" };
        string[] lastNames = { "Smith", "Doe", "Johnson", "Brown", "Taylor", "Anderson" };
        string firstName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
        string lastName = lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
        return $"{firstName} {lastName}";
    }

    public void SelectPerson()
    {
        if(this == PeopleManager.Instance.currentMostEvilPerson)
        {
            Debug.Log("Most Evil Person!");
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }
}
