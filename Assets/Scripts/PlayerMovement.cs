using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected float gravityMultiplier;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float groundDrag;

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


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        grounded = Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, flatGround);

        if (grounded)
            rb.drag = groundDrag;

        else
            rb.drag = 0;

        PlayerInput();
    }

    protected virtual void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    protected virtual void MovePlayer()
    {

        moveDirection = Vector3.Normalize(orientation.forward * verticalInput + orientation.right * horizontalInput);
        if (grounded)
        {
            rb.AddForce(Vector3.ProjectOnPlane(moveDirection * moveSpeed * 10f, transform.up), ForceMode.Force);
        }
    }
    protected virtual void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }
    protected virtual void SpeedControl()
    {
        Vector3 flatVel = Vector3.ProjectOnPlane(rb.velocity, transform.up);
        Vector3 upVel = Vector3.Project(rb.velocity, transform.up);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = upVel + limitedVel;
        }
    }

}
