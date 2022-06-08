using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Net;
#endif



public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public bool isPaused = false;

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 12f;

    // Crosshair
    public bool lockCursor = true;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    #endregion

    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    // Internal Variables
    private bool isZoomed = false;
    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    // Internal Variables
    private bool isWalking = false;

    #endregion

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    // Sprint Bar
    private bool useSprintBar = false;
    public bool hideBarWhenFull = true;
    public Image sprintBar;

    // Internal Variables
    private CanvasGroup sprintBarCG;
    private bool isSprinting = false;
    private float sprintRemaining;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    #endregion

    public Transform joint;

    // Internal Variables
    private Vector3 jointOriginalPos;


    // FUNCTION TO SAVE PLAYER POSITION 
    // private void LoadPlayer() {
    //     PlayerData savedData = SaveSystem.LoadPlayer();
    //     transform.position = new Vector3(savedData.position[0], savedData.position[1], savedData.position[2]);
    //     //TODO save and load look at direction
    // }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Set internal variables
        playerCamera.fieldOfView = fov;
        //originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;

        if (!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;
            sprintCooldownReset = sprintCooldown;
        }
    }

    private void Update()
    {
        if (!isPaused)
        {
            #region Camera

            // Control camera movement
            if (cameraCanMove)
            {
                yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

                if (!invertCamera)
                {
                    pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
                }
                else
                {
                    // Inverted Y
                    pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
                }

                // Clamp pitch between lookAngle
                pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

                transform.localEulerAngles = new Vector3(0, yaw, 0);
                playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            }
            #endregion

            #region Camera Zoom

            if (enableZoom)
            {
                // Changes isZoomed when key is pressed
                // Behavior for toogle zoom
                if (Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
                {
                    if (!isZoomed)
                    {
                        isZoomed = true;
                    }
                    else
                    {
                        isZoomed = false;
                    }
                }

                // Changes isZoomed when key is pressed
                // Behavior for hold to zoom
                if (holdToZoom && !isSprinting)
                {
                    if (Input.GetKeyDown(zoomKey))
                    {
                        isZoomed = true;
                    }
                    else if (Input.GetKeyUp(zoomKey))
                    {
                        isZoomed = false;
                    }
                }

                // Lerps camera.fieldOfView to allow for a smooth transistion
                if (isZoomed)
                {
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
                }
                else if (!isZoomed && !isSprinting)
                {
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
                }
            }

            #endregion

            #region Sprint

            if (enableSprint)
            {
                if (isSprinting)
                {
                    isZoomed = false;
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);

                    // Drain sprint remaining while sprinting
                    if (!unlimitedSprint)
                    {
                        sprintRemaining -= 1 * Time.deltaTime;
                        if (sprintRemaining <= 0)
                        {
                            isSprinting = false;
                            isSprintCooldown = true;
                        }
                    }
                }
                else
                {
                    // Regain sprint while not sprinting
                    sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
                }

                // Handles sprint cooldown 
                // When sprint remaining == 0 stops sprint ability until hitting cooldown
                if (isSprintCooldown)
                {
                    sprintCooldown -= 1 * Time.deltaTime;
                    if (sprintCooldown <= 0)
                    {
                        isSprintCooldown = false;
                    }
                }
                else
                {
                    sprintCooldown = sprintCooldownReset;
                }

                // Handles sprintBar 
                if (useSprintBar && !unlimitedSprint)
                {
                    float sprintRemainingPercent = sprintRemaining / sprintDuration;
                    sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
                }
            }

            #endregion
        }

    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            if (playerCanMove)
            {
                // Calculate how fast we should be moving
                Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                // Checks if player is walking and isGrounded
                // Will allow head bob
                if (targetVelocity.x != 0 || targetVelocity.z != 0)
                {
                    isWalking = true;
                }
                else
                {
                    isWalking = false;
                }

                // All movement calculations shile sprint is active
                if (enableSprint && Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown)
                {
                    targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                    // Apply a force that attempts to reach our target velocity
                    Vector3 velocity = rb.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    velocityChange.y = 0;

                    // Player is only moving when valocity change != 0
                    // Makes sure fov change only happens during movement
                    if (velocityChange.x != 0 || velocityChange.z != 0)
                    {
                        isSprinting = true;
                    }

                    rb.AddForce(velocityChange, ForceMode.VelocityChange);
                }
                // All movement calculations while walking
                else
                {
                    isSprinting = false;

                    if (hideBarWhenFull && sprintRemaining == sprintDuration)
                    {
                        sprintBarCG.alpha -= 3 * Time.deltaTime;
                    }

                    targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                    // Apply a force that attempts to reach our target velocity
                    Vector3 velocity = rb.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    velocityChange.y = 0;

                    rb.AddForce(velocityChange, ForceMode.VelocityChange);
                }
            }
        }
        
    }
    
    
}