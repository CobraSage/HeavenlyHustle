using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour
{
    [SerializeField] GameObject LoreScreen;
    [SerializeField] GameObject MainScreen;
    private void Start()
    {
        LoreScreen.SetActive(false);
        MainScreen.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OpenLore()
    {
        LoreScreen.SetActive(true);
        MainScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
