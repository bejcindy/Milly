using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoKeyWalk : MonoBehaviour
{
    public float verticalMove;

    public float moveSpeed;
    public float groundDrag;
    public float playerHeight;
    public Transform orientation;
    Vector3 moveDirection;
    Rigidbody rb;

    [SerializeField] protected bool grounded;
    [SerializeField] protected LayerMask flatGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            verticalMove = 1;
            if (Input.GetKeyDown(KeyCode.E)){
                verticalMove = 2;
            }

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            verticalMove = 1;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                verticalMove = 2;
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            verticalMove = 1;
        }
        
        if (grounded)
        {
            moveDirection = Vector3.Normalize(orientation.forward * 1);
            rb.AddForce(Vector3.ProjectOnPlane(moveDirection * moveSpeed * 10f, orientation.up), ForceMode.Force);
        }
    }
}
