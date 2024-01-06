using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;
using FMOD;

public class StartMenu : MonoBehaviour
{
    public bool startAllowed;
    public bool fadeSound;
    public Animator cinemachineAnim;
    public Animator startButtonAnim;
    public Animator quitButtonAnim;
    public EventSystem eventSystem;
    Animator canvasAnim;
    float fadeVal;

    [SerializeField] private EventReference SelectAudio;
    private EventInstance Audio;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fadeVal = 0f;
        canvasAnim = GetComponent<Animator>();
        Audio = RuntimeManager.CreateInstance(SelectAudio);
        RuntimeManager.AttachInstanceToGameObject(Audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
        Audio.start();
        Audio.release();

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

        if (startAllowed && Input.anyKeyDown)
        {
            fadeSound = true;
            RuntimeManager.PlayOneShot("event:/Sound Effects/UI/StartGame");
            StartGame();
        }

        if (fadeSound)
        {
            if(fadeVal < 1)
                fadeVal += Time.deltaTime * 0.1f;
            else
            {
                Audio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            Audio.setParameterByName("FadeOut", fadeVal);
        }


    }

    public void AllowStartGame()
    {
        startAllowed = true;
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
