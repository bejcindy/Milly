using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using FMODUnity;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public static bool pauseAfterCD;
    float pauseCD = 0.5f;
    public GameObject pauseMenu;
    Animator pauseMenuAnim;
    public GameObject player;
    public bool inTattoo;
    PlayerHolding playerHolding;

    readonly string pauseMenuSFX = "event:/Sound Effects/UI/PauseMenuOn";
    // Start is called before the first frame update
    void Start()
    {
        player = ReferenceTool.player.gameObject;
        playerHolding = ReferenceTool.playerHolding;
        ReferenceTool.pauseMenu = this;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseAfterCD = false;
        pauseMenu.transform.SetAsLastSibling();
        pauseMenuAnim = pauseMenu.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (StandardUISubtitlePanel panel in DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels)
        {
            if (panel.continueButton != null) panel.continueButton.interactable = !isPaused;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !MindPalace.tatMenuOn)
        {
            isPaused = !isPaused;
            pauseAfterCD = true;
            if (isPaused)
            {
                RuntimeManager.PlayOneShot("event:/Sound Effects/UI/PauseMenuOn", Camera.main.transform.position);
                PixelCrushers.UIPanel.monitorSelection = false; // Don't allow dialogue UI to steal back input focus.
                PixelCrushers.UIButtonKeyTrigger.monitorInput = false; // Disable hotkeys.
                PixelCrushers.DialogueSystem.DialogueManager.Pause(); // Stop DS timers (e.g., sequencer commands).
                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);

            }
            else
            {
                Time.timeScale = 1.0f;
                pauseMenuAnim.SetTrigger("Off");
                PixelCrushers.UIPanel.monitorSelection = true; // Allow dialogue UI to steal back input focus again.
                PixelCrushers.UIButtonKeyTrigger.monitorInput = true; // Re-enable hotkeys.
                PixelCrushers.DialogueSystem.DialogueManager.Unpause(); // Resume DS timers (e.g., sequencer commands).

                if (!playerHolding.positionFixedWithMouse && !MindPalace.tatMenuOn)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                if (pauseMenuAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && pauseMenuAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PauseMenuOff")
                {
                    pauseMenu.SetActive(false);
                }



            }
        }

        if (pauseAfterCD)
        {
            if (pauseCD > 0)
            {
                pauseCD -= Time.deltaTime;
            }
            else
            {
                pauseCD = 0.5f;
                pauseAfterCD = false;
            }
        }


        if (!isPaused)
        {
            if(pauseMenuAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                pauseMenu.SetActive(false);
            }
            if (!playerHolding.positionFixedWithMouse && !MindPalace.tatMenuOn)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
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
