using Beautify.Universal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPalace : MonoBehaviour
{
    public static bool tatMenuOn;
    public bool menuMoving;
    public CharacterTattooMenu currentMenu;


    Camera frontCam;
    Camera colorCam;
    Camera focusCam;

    [TextArea]
    public string hint;

    // Start is called before the first frame update
    void Start()
    {
        tatMenuOn = false;
        frontCam = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        colorCam = Camera.main.transform.GetChild(1).GetComponent<Camera>();
        focusCam = Camera.main.transform.GetChild(2).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && !menuMoving)
        {
            tatMenuOn = !tatMenuOn;

            if (!tatMenuOn)
            {
                MenuMouseHintOff();
                if (currentMenu)
                    currentMenu.TurnOffMenu();
            }
            else
            {
                if (currentMenu)
                {
                    currentMenu.TurnOnMenu();
                }
            }
        }
    }

    public void MenuMouseHintOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DataHolder.ShowHint(hint);

    }

    void MenuMouseHintOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        DataHolder.HideHint(hint);

    }


}
