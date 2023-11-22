using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;
    CinemachineBrain cameraBrain;

    // Start is called before the first frame update
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
    }   

    // Update is called once per frame
    void Update()
    {
        //float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        //float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //yRotation += mouseX;
        //xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        ////transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        orientation.forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
        transform.forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

    }

    private float GetAxisCustom(string axisName)
    {
        if (cameraBrain.IsBlending)
            return 0;
        return Input.GetAxis(axisName);
    }
}
