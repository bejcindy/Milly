using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashLid : MonoBehaviour
{
    public bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(0))
        {
            Debug.Log(transform.localEulerAngles.z);
            if(transform.localEulerAngles.z > 270 || transform.localEulerAngles.z < 1)
            {
                transform.Rotate(new Vector3(0, 0, verticalInput * -30) * Time.deltaTime);

            }


        }
    }
}
