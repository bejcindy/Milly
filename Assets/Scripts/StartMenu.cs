using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public bool startAllowed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startAllowed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().SetTrigger("Start");
            }
        }
    }

    public void EnableStart()
    {
        startAllowed = true;
    }

    public void LoadIzakayaScene()
    {
        SceneManager.LoadScene(1);
    }
}
