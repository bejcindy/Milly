using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected float gravityMultiplier;

    [SerializeField] GameObject upperRay;
    [SerializeField] GameObject lowerRay;
    [SerializeField] float stepHeight = 0.5f;
    [SerializeField] float stepSmooth = 10f;


    [Header("Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float groundDrag;

    [Header("Jumping")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float jumpCooldown;
    [SerializeField] protected float airMultiplier;
    [SerializeField] protected bool readyToJump;
    [SerializeField] protected KeyCode jumpKey = KeyCode.Space;

    protected float horizontalInput;
    protected float verticalInput;


    [Header("References")]
    [SerializeField] protected Transform orientation;
    protected Vector3 moveDirection;
    protected Rigidbody rb;


    [Header("Ground Check")]
    [SerializeField] protected float playerHeight;
    [SerializeField] protected bool grounded;
    [SerializeField] protected LayerMask flatGround;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lowerRay.transform.localPosition= new Vector3(0, -1.45f, 0);
        upperRay.transform.position = new Vector3(upperRay.transform.position.x, lowerRay.transform.position.y+stepHeight, upperRay.transform.position.z);
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, flatGround);

        if (grounded)
            rb.drag = groundDrag;

        else
            rb.drag = 0;

        PlayerInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
        walkStair();
    }

    void MovePlayer()
    {

        moveDirection = Vector3.Normalize(orientation.forward * verticalInput + orientation.right * horizontalInput);
        if (grounded)
        {
            rb.AddForce(Vector3.ProjectOnPlane(moveDirection * moveSpeed * 10f, orientation.up), ForceMode.Force);
        }
        //in air
        else if (!grounded)
        {
            rb.AddForce(Vector3.ProjectOnPlane(moveDirection * moveSpeed * 10f * airMultiplier, orientation.up), ForceMode.Force);
        }
    }
    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput == 0 && verticalInput == 0)
            GetComponent<terraintry>().startPainting = false;
        else
            GetComponent<terraintry>().startPainting = true;
        if (Input.GetKeyDown(jumpKey))
        {

            if (grounded && readyToJump)
            {
                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }

        }

    }
    void SpeedControl()
    {
        Vector3 flatVel = Vector3.ProjectOnPlane(rb.velocity, transform.up);
        Vector3 upVel = Vector3.Project(rb.velocity, transform.up);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = upVel + limitedVel;
        }
    }
    void walkStair()
    {
        Debug.Log("walking");
        RaycastHit hitLower;
        if (Physics.Raycast(lowerRay.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.5f, flatGround))
        {
            Debug.Log("hit lower");
            RaycastHit hitUpper;
            if (!Physics.Raycast(upperRay.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.7f, flatGround))
            {
                Debug.Log("hit upper");
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(lowerRay.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.5f, flatGround))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(upperRay.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.7f, flatGround))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(lowerRay.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.5f, flatGround))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(upperRay.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.7f, flatGround))
            {
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }
    protected virtual void Jump()
    {
        Vector3 verticalV = Vector3.ProjectOnPlane(rb.velocity, transform.right);
        Vector3 horizontalV = Vector3.ProjectOnPlane(rb.velocity, transform.up);
        rb.velocity = horizontalV + transform.up * jumpForce;
    }

    protected virtual void ResetJump()
    {
        readyToJump = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StoneLeft"))
        {
            other.transform.parent.GetComponent<FixedCameraObject>().onLeft = true;
        }
        if (other.CompareTag("StoneRight"))
        {
            other.transform.parent.GetComponent<FixedCameraObject>().onRight = true;
        }
        if (other.CompareTag("BuildingControl"))
        {
            other.transform.GetComponent<BuildingGroupController>().activateAll = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StoneLeft"))
        {
            other.transform.parent.GetComponent<FixedCameraObject>().onLeft = false;
        }
        if (other.CompareTag("StoneRight"))
        {
            other.transform.parent.GetComponent<FixedCameraObject>().onRight = false;
        }
    }

}
