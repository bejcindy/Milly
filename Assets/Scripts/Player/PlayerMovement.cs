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



    [Header("Sound")]
    protected FMOD.Studio.EventInstance playerMove;

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

    public bool initialCutsceneMove;
    public Transform currentTarget;

    bool ladderExited;
    Collider ladderTrigger;
    bool tooLeft, tooRight;
    PlayerHolding playerHolding;
    float movementAmount;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        lowerRay.transform.localPosition= new Vector3(0, -1f, 0);
        upperRay.transform.position = new Vector3(upperRay.transform.position.x, lowerRay.transform.position.y+stepHeight, upperRay.transform.position.z);
        playerMove = FMODUnity.RuntimeManager.CreateInstance("event:/Sound Effects/FootStep");
        playerHolding = GetComponent<PlayerHolding>();
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
        if ((onLadder && !grounded && !ladderExited) || (onLadder && verticalInput > 0 && grounded))
        {
            MoveOnLadder();
        }
        else
        {
            if (!initialCutsceneMove)
            {
                MovePlayer();
            }
            else
            {
                InitialCutsceneMovement(currentTarget);
            }

            SpeedControl();
        }
        if (!onLadder && grounded)
        {
            ladderExited = false;
            if (ladderTrigger)
            {
                if (!ladderTrigger.isTrigger)
                {
                    ladderTrigger.isTrigger = true;
                    ladderTrigger.gameObject.layer = 0;
                }
            }
        }

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
        if (collision.gameObject.name.Contains("walk_side"))
        {
            Maluyazi(collision.contacts[0].normal);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("slope"))
        {
            slopeCollide = false;
        }
    }

    void Maluyazi(Vector3 hitNormal)
    {
        //Debug.Log(Vector3.Dot(hitNormal, Vector3.up));
        //Debug.Log(rb.velocity);
        if (Mathf.Abs(Vector3.Dot(hitNormal, Vector3.up)) < .4f)
        {
            if (Vector3.Dot(moveDirection, -hitNormal) > 0 && rb.velocity.y < 0.5f)
            {
                //rb.position += new Vector3(0f, 0.3f, 0f);
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
                rb.position += transform.forward * Time.deltaTime;
            }
        }
    }

    void MoveOnLadder()
    {
        if (rb.useGravity)
            rb.useGravity = false;
        //rb.AddForce(Vector3.up * moveSpeed * verticalInput);
        float ySpeed = moveSpeed * verticalInput;
        //Vector3 horizontalDir = Vector3.ProjectOnPlane(transform.position - ladderTrigger.transform.position, ladderTrigger.transform.forward);
        //Vector3 horizontalDir=Vector3.Project(transform.position - ladderTrigger.transform.position, ladderTrigger.transform.right);
        float clampedHorizontalInput;
        if (tooLeft)
            clampedHorizontalInput = Mathf.Clamp(horizontalInput, 0, 1);
        else if (tooRight)
            clampedHorizontalInput = Mathf.Clamp(horizontalInput, -1, 0);
        else
            clampedHorizontalInput = horizontalInput;

        Vector3 xSpeed = moveSpeed * clampedHorizontalInput * -ladderTrigger.transform.right;
        Debug.Log("left: " + tooLeft + "right: " + tooRight + "clamped: " + clampedHorizontalInput);
        //Vector3 xSpeedZeroY = new Vector3(xSpeed.x, 0, xSpeed.z);
        //rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z);
        rb.velocity = new Vector3(xSpeed.x, ySpeed, xSpeed.z);
        
    }

    void MovePlayer()
    {
        RaycastHit ladderDetect;
        if(Physics.Raycast(transform.position, -transform.up, out ladderDetect, playerHeight, flatGround))
        {
            if (ladderDetect.transform.CompareTag("Ladder"))
            {
                rb.useGravity = false;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
            else
            {
                if (!rb.useGravity)
                    rb.useGravity = true;
            }
        }
        else
        {
            if(!rb.useGravity)
                rb.useGravity = true;
        }

        moveDirection = Vector3.Normalize(orientation.forward * verticalInput + orientation.right * horizontalInput);


        if (OnSlope()||slopeCollide)
        {
            if (verticalInput == 0 && horizontalInput == 0)
                rb.velocity = Vector3.zero;
            else
            {
                if (OnSlope())
                {
                    rb.AddForce(GetSlopeMoveDir() * moveSpeed * 20f, ForceMode.Force);

                }
                else if (slopeCollide)
                {
                    rb.AddForce(collisionSlopeDir * moveSpeed * 20f, ForceMode.Force);

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

    public void InitialCutsceneMovement(Transform target)
    {
        float clampedVert = Mathf.Clamp(verticalInput, 0, 1);
        moveDirection = Vector3.Normalize((target.position - transform.position) * clampedVert);

        if (OnSlope() || slopeCollide)
        {
            if (verticalInput == 0 && horizontalInput == 0)
                rb.velocity = Vector3.zero;
            else
            {
                if (OnSlope())
                {
                    rb.AddForce(GetSlopeMoveDir() * moveSpeed * 20f, ForceMode.Force);

                }
                else if (slopeCollide)
                {
                    rb.AddForce(collisionSlopeDir * moveSpeed * 20f, ForceMode.Force);

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
    }

    public bool CheckMoveAmount()
    {
        return (moveDirection.x > 2f || moveDirection.z > 2f);
    }



    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput == 0 && verticalInput == 0)
            GetComponent<terraintry>().startPainting = false;
        else
            GetComponent<terraintry>().startPainting = true;

        if (horizontalInput == 0 && verticalInput == 0 && grounded || playerHolding.inDialogue)
        {
            playerMove.setPaused(true);
        }
        else if (grounded)
        {
            FMOD.Studio.PLAYBACK_STATE state;
            playerMove.getPlaybackState(out state);
            bool isPaused;
            playerMove.getPaused(out isPaused);
            if (isPaused)
            {
                playerMove.setPaused(false);
            }
            else if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                playerMove.start();
            }
        }
        else if (!grounded)
        {
            playerMove.setPaused(true);
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
        {
            onLadder = true;
            if (!ladderTrigger)
            {
                Collider[] ladderColls = other.GetComponents<Collider>();
                if (ladderColls[0].isTrigger)
                {
                    ladderTrigger = ladderColls[0];
                    Debug.Log("trigger1");
                }
                else
                {
                    ladderTrigger = ladderColls[1];
                    Debug.Log("trigger2");
                }
                other.gameObject.layer = 6;
                //ladderTrigger = other.GetComponent<Collider>();
            }
                
        }
        if (other.name == "TooLeft")
            tooLeft = true;
        if (other.name == "TooRight")
            tooRight = true;
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
        {
            onLadder = false;
            ladderExited = true;

            //ladderTrigger.isTrigger = false;
            //Debug.Log("ladder is: " + ladderTrigger.isTrigger);
            //ladderTrigger.gameObject.layer = 6;
            if ((other.transform.position - transform.position).y < 0)
            {
                rb.AddForce(Vector3.ProjectOnPlane( orientation.forward,Vector3.up) * 200f);
                Debug.Log("added");
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
        if (other.name == "TooLeft")
            tooLeft = false;
        if (other.name == "TooRight")
            tooRight = false;
    }




    public void OpenSurvey()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdGedzEMk6VjuD2LUdROEXt9NoZFA0d4cO-gDnwiGO8Hh1qgA/viewform?usp=sf_link");
        Application.Quit();
    }

}
