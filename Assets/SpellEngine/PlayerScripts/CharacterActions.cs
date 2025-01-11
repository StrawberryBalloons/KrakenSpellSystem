using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using Unity.Netcode;

public class CharacterActions : MonoBehaviour
{
    // PLAYER
    private Rigidbody rb;
    public LayerMask playerLayer;

    // CAMERA
    public Slider slider;
    public GameObject playerCameraCentre;
    private float rotationX = 0;
    private float rotationY = 0;
    public float rotationSpeed = 3f;
    public float rotationSmoothness = 10f;
    public Camera[] cameras; // Array of cameras
    private int camIndex = 0;
    public KeyCode cameraKey = KeyCode.C;

    // CANVAS
    public Canvas uiCanvas; // The Canvas you want to control
    private CanvasScaler canvasScaler;

    // MOVEMENT - BASIC
    public float maxGroundedSpeed = 10f;
    public float walkSpeed = 5f;
    public float moveSpeed = 5f;
    public float deceleration = 1f;
    public KeyCode walkKey = KeyCode.LeftShift;
    float horizontalMultiplier = 0.5f; // Reduce side to side movement speed
    float backwardMultiplier = 0.7f; // Reduce backward movement speed

    // JUMP
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForce = 7.0f;
    private bool isGrounded = true;

    // // HANG
    // public KeyCode hangKey = KeyCode.H;
    // public float wallHangTime = 2f;
    // private bool isHanging;
    // private float hangTimer;
    // private Transform hangingWall;

    // // LEDGE
    // public KeyCode ledgeGrabKey = KeyCode.L;
    // public float ledgeDetectionRange = 0.5f;
    // private float grabCooldown = 0.5f;
    // private bool canGrab = true;

    // INTERACT
    public float reachDistance = 10f;
    public bool screenClick = false;
    public KeyCode interactKey = KeyCode.Mouse1;
    public Interactable focus;
    public Transform lookingAt;

    public bool ReturnIsGrounded()
    {
        return isGrounded;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation so we can control it manually

        slider.onValueChanged.AddListener(delegate { SetCameraSpeed(slider.value); });

        // Ensure only the first camera is active at the start
        SwitchCamera(0);

        if (uiCanvas != null)
        {
            canvasScaler = uiCanvas.GetComponent<CanvasScaler>();
        }
    }

    void FixedUpdate()
    {
        CheckGrounded(); // Check if the player is grounded using raycast
        if (!Cursor.visible)
        {
            if (Input.GetKeyDown(jumpKey) && isGrounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(cameraKey))
            {
                SwitchPerspective();
            }

            HandleInput();

            Rotate();

        }
    }

    void Update()
    {
        Interact();
    }

    void Interact()
    {
        if (Input.GetKeyDown(interactKey))
        {
            RemoveFocus();
            Ray ray = screenClick
                ? Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0))
                : new Ray(playerCameraCentre.transform.position, cameras[camIndex].transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, reachDistance, ~playerLayer))
            {
                Debug.Log("Hit: " + hitInfo.collider.name);
                lookingAt = hitInfo.transform;
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            focus?.OnDefocused();
            focus = newFocus;
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        focus?.OnDefocused();
        focus = null;
    }

    public void SetCameraSpeed(float camRotSpe)
    {
        rotationSpeed = camRotSpe;
    }

    void HandleInput()
    {

        // if (Input.GetKeyDown(hangKey) && !isGrounded)
        // {
        //     TryHangOnWall();
        //     actionFlag = true;
        // }

        // if (canGrab && Input.GetKeyDown(ledgeGrabKey))
        // {
        //     TryGrabLedge();
        //     actionFlag = true;
        // }


        HandleMovementInput();
    }

    void SwitchPerspective()
    {
        camIndex = (camIndex + 1) % cameras.Length;
        SwitchCamera(camIndex);
    }
    void SetCanvasRenderCamera(Camera activeCamera)
    {
        // Set the Canvas' RenderCamera to the active camera
        if (uiCanvas != null)
        {
            uiCanvas.worldCamera = activeCamera;
        }
    }

    void SwitchCamera(int index)
    {

        // Disable all cameras
        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        if (index >= 0 && index < cameras.Length)
        {
            cameras[index].gameObject.SetActive(true);
            SetCanvasRenderCamera(cameras[index]);
        }
    }

    // void HandleMovementInput()
    // {
    //     float moveHorizontal = Input.GetAxis("Horizontal");
    //     float moveVertical = Input.GetAxis("Vertical");

    //     Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

    //     // Adjust maxGroundedSpeed for walking
    //     float currentSpeed = Input.GetKey(walkKey) ? walkSpeed : maxGroundedSpeed;

    //     if (isGrounded)
    //     {
    //         Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    //         if (horizontalVelocity.magnitude > currentSpeed)
    //         {
    //             rb.velocity = new Vector3(horizontalVelocity.normalized.x * currentSpeed, rb.velocity.y, horizontalVelocity.normalized.z * currentSpeed);
    //         }

    //         if (movement.magnitude > 0)
    //         {
    //             rb.AddForce(movement * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    //         }
    //         else
    //         {
    //             Vector3 decelerationForce = -rb.velocity.normalized * deceleration * Time.fixedDeltaTime;
    //             rb.AddForce(decelerationForce, ForceMode.VelocityChange);
    //         }
    //     }
    // }
    void HandleMovementInput()
    {

        float currentSpeed = Input.GetKey(walkKey) ? walkSpeed : maxGroundedSpeed;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get the forward and right vectors of the camera
        Vector3 cameraForward = playerCameraCentre.transform.forward;
        Vector3 cameraRight = playerCameraCentre.transform.right;

        // Project the camera vectors onto the horizontal plane (y = 0)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the vectors
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Apply the multipliers
        if (moveVertical < 0)
        {
            moveVertical *= backwardMultiplier;
        }
        moveHorizontal *= horizontalMultiplier;

        // Calculate the movement direction in world space
        Vector3 movement = (cameraForward * moveVertical + cameraRight * moveHorizontal).normalized;

        // Apply movement to the Rigidbody if grounded
        if (isGrounded)
        {
            // Limit maximum velocity while grounded
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            float horizontalSpeed = horizontalVelocity.magnitude;
            if (horizontalSpeed > currentSpeed)
            {
                // Calculate the velocity vector with the same direction but capped magnitude
                Vector3 clampedVelocity = horizontalVelocity.normalized * currentSpeed;
                rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
            }

            // Apply movement force if there is a movement direction
            if (movement.magnitude > 0)
            {
                rb.AddForce(movement * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
            else
            {
                // Apply grounded deceleration if no movement direction is detected
                Vector3 decelerationForce = -rb.velocity.normalized * deceleration * Time.fixedDeltaTime;
                rb.AddForce(decelerationForce, ForceMode.VelocityChange);
            }
        }
        else
        {
            // If not grounded, add force for more fluid movement in the air
            float clampedSpeed = Mathf.Clamp(moveSpeed, 1f, float.MaxValue) / 10f;
            rb.AddForce(movement * clampedSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }


    // void Move() { }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationY += mouseX;
        rotationX = Mathf.Clamp(rotationX + mouseY, -90, 90);

        Quaternion targetCameraRotation = Quaternion.Euler(rotationX, 0, 0);
        playerCameraCentre.transform.localRotation = Quaternion.Lerp(playerCameraCentre.transform.localRotation, targetCameraRotation, Time.fixedDeltaTime * rotationSmoothness);

        Quaternion playerTargetRotation = Quaternion.Euler(0, rotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRotation, Time.fixedDeltaTime * rotationSmoothness);
    }

    // void RotateAround()
    // {
    //     Vector3 rotationPoint = transform.position;
    //     float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
    //     float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

    //     rotationY += mouseX;
    //     Quaternion playerTargetRotation = Quaternion.Euler(0, rotationY, 0);
    //     transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRotation, Time.fixedDeltaTime * rotationSmoothness);

    //     Vector3 right = playerCameraCentre.transform.right;
    //     playerCameraCentre.transform.RotateAround(rotationPoint, right, mouseY);
    // }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out _, 1.3f, ~playerLayer);
    }

    // void TryGrabLedge()
    // {
    //     if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, ledgeDetectionRange, ~playerLayer))
    //     {
    //         StartCoroutine(GrabLedge(hit.point, hit.normal));
    //     }
    // }

    // IEnumerator GrabLedge(Vector3 ledgePosition, Vector3 ledgeNormal)
    // {
    //     canGrab = false;
    //     transform.position = ledgePosition;
    //     transform.rotation = Quaternion.LookRotation(-ledgeNormal);
    //     yield return new WaitForSeconds(grabCooldown);
    //     canGrab = true;
    // }

    // void TryHangOnWall()
    // {
    //     if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.5f, ~playerLayer))
    //     {
    //         HangOnWall(hit.transform);
    //     }
    // }

    // void HangOnWall(Transform wall)
    // {
    //     isHanging = true;
    //     hangTimer = 0f;
    //     hangingWall = wall;
    //     rb.velocity = Vector3.zero;
    // }
}
