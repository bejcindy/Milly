using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTattooMesh : MonoBehaviour
{
    public bool draggingTattoo;
    public bool rotatingNPC;
    public float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!draggingTattoo && Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
            transform.Rotate(Vector3.up, -rotX);
            rotatingNPC = true;
        }
        else
        {
            rotatingNPC = false;
        }

    }
}
