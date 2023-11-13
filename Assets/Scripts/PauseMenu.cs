using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public static bool pauseAfterCD;
    float pauseCD = 2f;
    public GameObject pauseMenu;
    public GameObject player;
    PlayerHolding playerHolding;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerHolding = player.GetComponent<PlayerHolding>();
    }

    // Update is called once per frame
    void Update()
    {
        DialogueManager.displaySettings.defaultCanvas.GetComponent<GraphicRaycaster>().enabled = !isPaused;

        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = !isPaused;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
                isPaused = !isPaused;
        }

        if (isPaused)
        {
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);

        }
        else
        {
            DialogueManager.Unpause();

            if (!playerHolding.positionFixedWithMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);

            if (pauseCD > 0)
            {
                pauseAfterCD = true;
                pauseCD -= Time.deltaTime;
            }
            else
            {
                pauseCD = 2f;
                pauseAfterCD = false;
            }
        }
    }

    IEnumerator ClearSelectionAfterOneFrame() // Could select a pause menu button instead.
    {
        yield return null;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);

        }
        else
        {
            
            isPaused = true;
            Time.timeScale = 0.0f;
            StartCoroutine(ClearSelectionAfterOneFrame());

        }
    }

    public void QuitGame()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdGedzEMk6VjuD2LUdROEXt9NoZFA0d4cO-gDnwiGO8Hh1qgA/viewform?usp=sf_link");
        Application.Quit(); 

    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        StartSequence.noControl = false;
        MainQuestState.ResetVars();
    }
}
