using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public bool startAllowed;
    public Animator cinemachineAnim;
    public Animator startButtonAnim;
    public Animator quitButtonAnim;
    public EventSystem eventSystem;
    Animator canvasAnim;
    // Start is called before the first frame update
    void Start()
    {
        canvasAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachineAnim.isActiveAndEnabled)
        {
            if(cinemachineAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                LoadIzakayaScene();
            }
        }

        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }

    public void EnableStart()
    {
        startAllowed = true;
    }

    public void EnableButtonAnim()
    {
        startButtonAnim.enabled = true;
        quitButtonAnim.enabled = true;
    }


    public void DisableButtonAnim()
    {
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        //startButtonAnim.enabled = false;
        //quitButtonAnim.enabled = false;
    }
    public void LoadIzakayaScene()
    {
        SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        canvasAnim.SetTrigger("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartCameraLerp()
    {
        cinemachineAnim.enabled = true;
    }
}
