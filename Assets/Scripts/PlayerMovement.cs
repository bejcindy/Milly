using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cinemachine.Utility;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected float gravityMultiplier;
    public bool onLadder;

    [Header("SlopeCheck")]
    [SerializeField] bool onSlope;
    public RaycastHit slopeHit;
    public float maxSlopeAngle;
    [SerializeField] bool slopeCollide;
    Vector3 collisionSlopeDir;

    [Header("Stair Related")]
    [SerializeField] GameObject upperRay;
    [SerializeField] GameObject lowerRay;
    [SerializeField] float stepHeight = 0.5f;
    [SerializeField] float stepSmooth = 15f;
    [SerializeField] float upperDetect = .7f;
    [SerializeField] float lowerDetect = .7f;

    
    public float step_LookAheadRange = 0.1f;
    public float step_MaxStepHeight = 0.3f;
    public float playerRadius;
    public float slopeLimit;

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
    [SerializeField] protected LayerMask stairs;
    [SerializeField] protected float stairForce;

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lowerRay.transform.localPosition= new Vector3(0, -1f, 0);
        upperRay.transform.position = new Vector3(upperRay.transform.position.x, lowerRay.transform.position.y+stepHeight, upperRay.transform.position.z);
    }

    void Update()
    {
        
        grounded = Physics.Raycast(transform.position, -transform.up, playerHeight, flatGround);
        //upperRay.transform.position = new Vector3(upperRay.transform.position.x, lowerRay.transform.position.y + stepHeight, upperRay.transform.position.z);
        if (grounded)
            rb.drag = groundDrag;

        else
            rb.drag = 0;

        PlayerInput();
        //Debug.DrawRay(lowerRay.transform.position, transform.forward * lowerDetect, Color.red);
        //Debug.DrawRay(upperRay.transform.position, transform.forward * upperDetect, Color.blue);
        ////if (horizontalInput != 0 && verticalInput != 0)
        //    walkStair();
    }

    void FixedUpdate()
    {
        onSlope = OnSlope();
        if (!onLadder)
        {
            MovePlayer();
            SpeedControl();
        }
        else
        {
            MoveOnLadder();
        }
        //if (horizontalInput != 0 && verticalInput != 0)
        //    walkStair();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("slope"))
        {
            slopeCollide = true;
            //RaycastHit downRay;
            //if (Physics.Raycast(transform.position, -transform.up, out downRay, -10f, flatGround))
            //    collisionSlopeDir = Vector3.ProjectOnPlane(moveDirection, downRay.normal).normalized;
            collisionSlopeDir = Vector3.ProjectOnPlane(moveDirection, collision.contacts[0].normal).normalized;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("slope"))
        {
            slopeCollide = false;
        }
    }

    void MoveOnLadder()
    {
        if (rb.useGravity)
            rb.useGravity = false;
        //rb.AddForce(Vector3.up * moveSpeed * verticalInput);
        float ySpeed = moveSpeed * verticalInput;
        rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z);
    }

    void MovePlayer()
    {
        if(!rb.useGravity)
            rb.useGravity = true;
        moveDirection = Vector3.Normalize(orientation.forward * verticalInput + orientation.right * horizontalInput);
        //Debug.Log(rb.velocity);
        //Debug.DrawRay(transform.position, GetSlopeMoveDir() * moveSpeed * 20f, Color.red);
        //Debug.DrawRay(transform.position, collisionSlopeDir * moveSpeed * 20f, Color.blue);
        if (OnSlope()||slopeCollide)
        {
            if (verticalInput == 0 && horizontalInput == 0)
                rb.velocity = Vector3.zero;
            else
            {
                if (OnSlope())
                {
                    rb.AddForce(GetSlopeMoveDir() * moveSpeed * 20f, ForceMode.Force);
                    //Debug.Log("onslope");
                }
                else if (slopeCollide)
                {
                    rb.AddForce(collisionSlopeDir * moveSpeed * 20f, ForceMode.Force);
                    //Debug.Log("colliding");
                }

                //if (rb.velocity.y > 0)
                //    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                if (rb.velocity.y > 0)
                    rb.AddForce(Vector3.down * 10f, ForceMode.Force);
                else if (rb.velocity.y < 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
        }

        if (grounded)
        {
            Vector3 movementVector = Vector3.ProjectOnPlane(moveDirection * moveSpeed * 10f, orientation.up);

            rb.AddForce(movementVector, ForceMode.Force);
            //CheckForStep(ref movementVector);

            walkStair(ref movementVector);

        }
        //in air
        else if (!grounded)
        {
            rb.AddForce(Vector3.ProjectOnPlane(moveDirection * moveSpeed * 10f * airMultiplier, orientation.up), ForceMode.Force);
        }

        if (OnSlope() || slopeCollide)
            rb.useGravity = false;
        else if (!OnSlope() && !slopeCollide)
            rb.useGravity = true;
                
        //rb.useGravity = !OnSlope();
        
    }

    void CheckForStep(ref Vector3 movement)
    {
        Vector3 lookAheadStartPoint = lowerRay.transform.position+ Vector3.up * (step_MaxStepHeight * 0.5f);
        Vector3 lookAheadDir = movement.normalized;
        float lookAheadDistance = playerRadius + step_LookAheadRange;
        if (Physics.Raycast(lookAheadStartPoint, lookAheadDir, lookAheadDistance, stairs))
        {
            Debug.Log("Checking for step and there is ");
            lookAheadStartPoint = lowerRay.transform.position + Vector3.up * step_MaxStepHeight;
            Debug.Log(lookAheadStartPoint);
            if (!Physics.Raycast(lookAheadStartPoint, lookAheadDir, lookAheadDistance, flatGround))
            {
                Vector3 candidatePoint = lookAheadStartPoint + lookAheadDir * lookAheadDistance;
                Debug.Log(candidatePoint);

                RaycastHit hitResult;
                if (Physics.Raycast(candidatePoint, Vector3.down, out hitResult, step_MaxStepHeight, flatGround))
                {
                    Debug.Log("Hit result is " + hitResult);
                    if (Vector3.Angle(Vector3.up, hitResult.normal) <= slopeLimit)
                    {
                        Debug.Log("final is " + hitResult.point + Vector3.up);
                        transform.position = hitResult.point + Vector3.up;
                    }
                }
            }
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
        if (OnSlope() || slopeCollide)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            //if (verticalInput == 0)
            //{
            //    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            //    Debug.Log("trying");
            //}
        }
        else if (!OnSlope() && !slopeCollide)
        {
            //Debug.Log("here");
            Vector3 flatVel = Vector3.ProjectOnPlane(rb.velocity, transform.up);
            Vector3 upVel = Vector3.Project(rb.velocity, transform.up);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = upVel + limitedVel;
            }
        }

    }


    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight+.3f, flatGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //Debug.Log(angle);
            return angle < maxSlopeAngle && angle != 0;
            //return angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDir()
    {
        

        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void walkStair(ref Vector3 movement)
    {
        //Debug.Log("walking");

        RaycastHit hitLower;
        if (Physics.Raycast(lowerRay.transform.position, movement.normalized, out hitLower, 0.3f, flatGround))
        {
            Debug.Log("hit lower");
            RaycastHit hitUpper;
            if (!Physics.Raycast(upperRay.transform.position, movement.normalized, out hitUpper, 0.3f, flatGround))
            {
                Debug.Log("stairing");
                rb.position -= new Vector3(0f, -0.5f, 0f);
                rb.position += transform.forward * Time.deltaTime;
            }
        }

        //RaycastHit hitLower45;
        //if (Physics.Raycast(lowerRay.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, lowerDetect, flatGround))
        //{

        //    RaycastHit hitUpper45;
        //    if (!Physics.Raycast(upperRay.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, upperDetect, flatGround))
        //    {
        //        rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
        //    }
        //}

        //RaycastHit hitLowerMinus45;
        //if (Physics.Raycast(lowerRay.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, lowerDetect, flatGround))
        //{

        //    RaycastHit hitUpperMinus45;
        //    if (!Physics.Raycast(upperRay.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, upperDetect, flatGround))
        //    {
        //        rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
        //    }
        //}
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
        if (other.CompareTag("Ladder"))
            onLadder = true;
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
        if (other.CompareTag("Ladder"))
            onLadder = false;
    }

}
