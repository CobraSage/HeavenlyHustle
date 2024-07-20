using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [field: Header("Currency Stuff")]
    public static CurrencyManager Instance { get; private set; }
    public int soulsAmount { get; private set; } = 0;
    public int happinessPointsAmount /*{ get; private set; }*/ = 0;

    [SerializeField] private TextMeshProUGUI soulsText;
    [SerializeField] private TextMeshProUGUI happinessPointsText;

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
        if (soulsText != null)
        {
            soulsText.text = soulsAmount.ToString();
        }
        else
        {
            UtilityScript.LogError("Souls Amount Text Not Assigned!");
            return;
        }

        if(happinessPointsText != null)
        {
        happinessPointsText.text = happinessPointsAmount.ToString();
        }
        else
        {
            UtilityScript.LogError("Happiness Points Amount Text Not Assigned!");
            return;
        }
    }

    public void AdjustSouls(int amount)
    {
        soulsAmount += amount;
        //saving&loading
    }

    public void AdjustHappinessPoints(int amount)
    {
        happinessPointsAmount += amount;
        //saving&loading
    }

    public int GetSoulsAmount()
    {
        return soulsAmount;
    }

    public int GetHappinessPointsAmount()
    {
        return happinessPointsAmount;
    }
}
