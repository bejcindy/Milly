using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSittingSwitch : MonoBehaviour
{

    public CinemachineVirtualCamera playerMainCam;
    public CinemachineVirtualCamera playerSitCam;

    List<FixedCameraObject> fixedCamObjs;

    public bool objInteracting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
