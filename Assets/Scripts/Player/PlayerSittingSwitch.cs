using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSittingSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera playerMainCam;
    public CinemachineVirtualCamera playerSitCam;

    PlayerMovement pm;

    public List<FixedCameraObject> fixedCamObjs;

    public bool objInteracting;
    public bool sitting;
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteractable();
        if (!objInteracting)
        {
            SwitchSitting();
        }
    }

    void SwitchSitting()
    {
        if (!sitting)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                sitting = true;
                SwitchSittingCam();
            }
        }
        else
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            if(horizontalInput > 0 || verticalInput > 0)
            {
                sitting = false;
                StartCoroutine(StandUp());
            }
        }
    }

    void CheckInteractable()
    {
        if(fixedCamObjs.Count <= 0)
        {
            objInteracting = false;
        }
        else
        {
            objInteracting = true;
        }
    }

    public void AddCamObj(FixedCameraObject obj)
    {
        if (!fixedCamObjs.Contains(obj))
        {
            fixedCamObjs.Add(obj);  
        }
    }

    public void RemoveCamObj(FixedCameraObject obj)
    {
        if (fixedCamObjs.Contains(obj))
        {
            fixedCamObjs.Remove(obj);
        }
    }


    void SwitchSittingCam()
    {
        pm.enabled = false;
        playerSitCam.m_Priority = 10;
        playerMainCam.m_Priority = 9;
    }

    protected IEnumerator StandUp()
    {
        playerMainCam.m_Priority = 10;
        playerSitCam.m_Priority = 9;

        yield return new WaitForSeconds(2f);
        playerSitCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = 0;
        playerSitCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 0;
        pm.enabled = true;

    }
}
