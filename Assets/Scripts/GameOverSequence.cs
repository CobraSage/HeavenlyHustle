using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSequence : MonoBehaviour
{
    public static GameOverSequence Instance { get; private set; }
    [field: Header("Game Over Sequence")]
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private Button tryAgainButton;

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
        GameOverUI.SetActive(false);
        tryAgainButton.onClick.AddListener(() => OnTryAgainClicked());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnGameOverSequenceInitiated();
        }
    }

    public void OnGameOverSequenceInitiated()
    {
        Time.timeScale = 0.0f;
        GameOverUI.SetActive(true);
        PersistentDataManager.Instance.DeleteAllSaveData();
    }

    private void OnTryAgainClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainGameScene");
    }
}
