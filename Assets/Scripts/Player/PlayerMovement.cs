using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cinemachine.Utility;
using UnityEngine;
using FMODUnity;
using Cinemachine;
using VInspector;

public class PlayerMovement : MonoBehaviour
{
    protected float gravityMultiplier;
    protected float horizontalInput;
    protected float verticalInput;

    [Foldout("Basic Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float maxWalkSpeed;
    [SerializeField] protected float groundDrag;
    [SerializeField] protected float playerHeight;
    public bool cameraLocked;

    [Foldout("Ground Check")]
    [SerializeField] protected bool grounded;
    [SerializeField] protected LayerMask flatGround;
    [SerializeField] protected LayerMask stairs;
    [SerializeField] protected float stairForce;

    [Foldout("SlopeCheck")]
    [SerializeField] bool onSlope;
    public RaycastHit slopeHit;
    public float maxSlopeAngle;
    [SerializeField] bool slopeCollide;
    Vector3 collisionSlopeDir;

    [Foldout("Stair Related")]
    [SerializeField] GameObject upperRay;
    [SerializeField] GameObject lowerRay;
    [SerializeField] float stepHeight = 0.5f;
    [SerializeField] float stepSmooth = 15f;
    [SerializeField] float upperDetect = .7f;
    [SerializeField] float lowerDetect = .7f;
    public bool onLadder;
    public float step_LookAheadRange = 0.1f;
    public float step_MaxStepHeight = 0.3f;
    public float playerRadius;
    public float slopeLimit;


    [Foldout("Sound")]
    public bool atInterior;
    public float fadeSpeed;
    public float currentVolume;
    private FMOD.Studio.EventInstance walkSFX;
    private FMOD.Studio.EventInstance outsideAmbience;




    [Foldout("References")]
    [SerializeField] protected Transform orientation;
    protected Vector3 moveDirection;
    protected Rigidbody rb;


    [Foldout("Glasses")]
    public GameObject[] glasses;



    public bool initialCutsceneMove;
    public Transform currentTarget;

    bool ladderExited;
    Collider ladderTrigger;
    bool tooLeft, tooRight;
    PlayerHolding playerHolding;
    float movementAmount;

    bool stopMoving;
    CinemachineVirtualCamera playerCam;
    CinemachineBrain camBrain;
    CinemachineBasicMultiChannelPerlin camNoise;
    [SerializeField] float breathingAmp;
    [SerializeField] float breathingFreq;
    [SerializeField] float walkingAmp;
    [SerializeField] float walkingFreq;
    public NoiseSettings breathingNoise;
    bool walking;
    public bool easingNoise = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
        lowerRay.transform.localPosition= new Vector3(0, -1f, 0);
        upperRay.transform.position = new Vector3(upperRay.transform.position.x, lowerRay.transform.position.y+stepHeight, upperRay.transform.position.z);

        playerHolding = ReferenceTool.playerHolding;
        playerCam = ReferenceTool.playerCinemachine;
        camBrain = ReferenceTool.playerBrain;
        playerCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camNoise = playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camNoise.m_NoiseProfile = breathingNoise;
        camNoise.m_AmplitudeGain = breathingAmp;
        camNoise.m_FrequencyGain = breathingFreq;

        walkSFX = FMODUnity.RuntimeManager.CreateInstance("event:/Sound Effects/FootStep");
        outsideAmbience = RuntimeManager.CreateInstance("event:/Static/Outside_Ambience");
        outsideAmbience.start();
        outsideAmbience.setVolume(0);
    }

    void Update()
    {

        grounded = Physics.Raycast(transform.position, -transform.up, playerHeight, flatGround);
        if (grounded)
            rb.drag = groundDrag;

        else
            rb.drag = 0;

        if(!stopMoving && !cameraLocked)
            PlayerInput();

        if(atInterior && currentVolume > 0)
        {
            FadeOutOutside();
        }

        if(!atInterior && currentVolume < 1)
        {
            FadeInOutside();
        }

        EaseInMovement();

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
            if (!MainTattooMenu.tatMenuOn)
                MovePlayer();
            else
                walking = false;

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
            collisionSlopeDir = Vector3.ProjectOnPlane(moveDirection, collision.contacts[0].normal).normalized;
        }
        if (collision.gameObject.name.Contains("road_sidebrisk"))
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
        float ySpeed = moveSpeed * verticalInput;
        float clampedHorizontalInput;
        if (tooLeft)
            clampedHorizontalInput = Mathf.Clamp(horizontalInput, 0, 1);
        else if (tooRight)
            clampedHorizontalInput = Mathf.Clamp(horizontalInput, -1, 0);
        else
            clampedHorizontalInput = horizontalInput;

        Vector3 xSpeed = moveSpeed * clampedHorizontalInput * -ladderTrigger.transform.right;
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

        if (moveDirection != Vector3.zero && !camBrain.IsBlending && rb.velocity.magnitude > 0.05f) 
        {
            walking = true;

        }
        else
        {
            walking = false;

        }


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
            walkStair(ref movementVector);

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

    public void ToBreathingNoise()
    {
        stopMoving = true;
        camNoise.m_NoiseProfile = breathingNoise;
    }

    private void OnConversationEnd(Transform other)
    {
        stopMoving = false;
    }

    public void OnDisable()
    {
        walkSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void EaseInMovement()
    {
        if (walking)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                walkSFX.setPitch(1.125f);
                walkSpeed = 4f;
                maxWalkSpeed = 5f;
            }
            else
            {
                walkSFX.setPitch(1);
                walkSpeed = 3;
                maxWalkSpeed = 4;
            }
            if (!atInterior)
            {

                if (moveSpeed < maxWalkSpeed)
                {
                    moveSpeed += Time.deltaTime*1.5f;
                }
                else
                {
                    moveSpeed = maxWalkSpeed;
                }
            }
            else
            {
                if (moveSpeed < walkSpeed)
                {
                    moveSpeed += Time.deltaTime * 1.5f;
                }
                else
                {
                    moveSpeed = walkSpeed;
                }
            }


            if (camNoise.m_AmplitudeGain < walkingAmp)
                camNoise.m_AmplitudeGain += Time.deltaTime;
            else
                camNoise.m_AmplitudeGain = walkingAmp;

            if(camNoise.m_FrequencyGain < walkingFreq)
                camNoise.m_FrequencyGain += Time.deltaTime * 2f;
            else
                camNoise.m_FrequencyGain = walkingFreq;
        }
        else
        {
            moveSpeed = 2;
            if (camNoise.m_AmplitudeGain > breathingAmp)
                camNoise.m_AmplitudeGain -= Time.deltaTime;
            else
                camNoise.m_AmplitudeGain = breathingAmp;

            if (camNoise.m_FrequencyGain > breathingFreq)
                camNoise.m_FrequencyGain -= Time.deltaTime * 2f;
            else
                camNoise.m_FrequencyGain = breathingFreq;
        }

    }

    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        if (!walking && grounded)
        {
            walkSFX.setPaused(true);
        }
        else if (grounded)
        {
            FMOD.Studio.PLAYBACK_STATE state;
            walkSFX.getPlaybackState(out state);
            bool isPaused;
            walkSFX.getPaused(out isPaused);
            if (isPaused)
            {
                walkSFX.setPaused(false);
            }
            else if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                walkSFX.start();
            }
        }
        else if (!grounded)
        {
            walkSFX.setPaused(true);
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
            RaycastHit hitUpper;
            if (!Physics.Raycast(upperRay.transform.position, movement.normalized, out hitUpper, 0.3f, flatGround))
            {
                rb.position -= new Vector3(0f, -0.5f, 0f);
                rb.position += transform.forward * Time.deltaTime;
            }
        }

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
                }
                else
                {
                    ladderTrigger = ladderColls[1];
                }
                other.gameObject.layer = 6;
                //ladderTrigger = other.GetComponent<Collider>();
            }
                
        }
        if (other.name == "TooLeft")
            tooLeft = true;
        if (other.name == "TooRight")
            tooRight = true;

        if (other.CompareTag("AmbienceOcclude"))
        {
            atInterior = true;
        }

        if (other.CompareTag("Fog"))
        {
            Debug.Log("Entered fog");
            foreach(GameObject glass in glasses)
            {
                gameObject.layer = 0;
            }
        }


        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fog"))
        {
            Debug.Log("In fog");
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
        if (other.CompareTag("Ladder"))
        {
            onLadder = false;
            ladderExited = true;

            //ladderTrigger.isTrigger = false;
            //Debug.Log("ladder is: " + ladderTrigger.isTrigger);
            //ladderTrigger.gameObject.layer = 6;
            if ((other.transform.position - transform.position).y < 0)
            {
                rb.AddForce(Vector3.ProjectOnPlane(orientation.forward, Vector3.up) * 200f);
                Debug.Log("added");
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
        if (other.name == "TooLeft")
            tooLeft = false;
        if (other.name == "TooRight")
            tooRight = false;


        if (other.CompareTag("AmbienceOcclude"))
        {
            atInterior = false;
        }

        if (other.CompareTag("Fog"))
        {
            Debug.Log("Exited Fog");
            foreach (GameObject glass in glasses)
            {
                gameObject.layer = 17;
            }
        }
    }

    
    void FadeInOutside()
    {
        if(currentVolume < 1)
        {
            currentVolume += Time.deltaTime * fadeSpeed;
        }
        else
        {
            currentVolume = 1;
        }
        outsideAmbience.setVolume(currentVolume);
    }

    void FadeOutOutside()
    {
        if(currentVolume > 0)
        {
            currentVolume -= Time.deltaTime * fadeSpeed;
        }
        else
        {
            currentVolume = 0;
        }
        outsideAmbience.setVolume(currentVolume);
    }

    public void OpenSurvey()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdGedzEMk6VjuD2LUdROEXt9NoZFA0d4cO-gDnwiGO8Hh1qgA/viewform?usp=sf_link");
        Application.Quit();
    }

}
