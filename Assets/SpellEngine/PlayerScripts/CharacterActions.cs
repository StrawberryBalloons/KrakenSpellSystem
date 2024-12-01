using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.EventSystems;
using Unity.Netcode;

public class CharacterActions : MonoBehaviour
{
    //PLAYER
    private Rigidbody rb;
    public LayerMask playerLayer;

    //CAMERA
    public Slider slider;
    public Camera playerCamera;
    private float rotationX = 0;
    private float rotationY = 0;
    public float rotationSpeed = 3f;
    public float rotationSmoothness = 10f;
    [System.Serializable]
    public struct CameraTransform
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    public CameraTransform[] camPositions;
    public KeyCode cameraKey = KeyCode.Space;
    private int camIndex = 0;

    //MOVEMENT - BASIC
    public float maxGroundedSpeed = 10f;
    public float moveSpeed = 5f;
    public float deceleration = 1f;// Set multipliers for different movement directions
    float horizontalMultiplier = 0.5f; // Reduce side to side movement speed
    float backwardMultiplier = 0.7f; // Reduce backward movement speed

    //JUMP
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpForce = 7.0f;
    private bool isGrounded;

    //HANG
    public KeyCode hangKey = KeyCode.H;
    public float wallHangTime = 2f;
    private bool isHanging;
    private float hangTimer;
    private Transform hangingWall;

    //LEDGE
    public KeyCode ledgeGrabKey = KeyCode.L;
    public float ledgeDetectionRange = 0.5f;
    private float grabCooldown = 0.5f;
    private bool canGrab = true;

    //INTERACT
    public float reachDistance = 10f;
    public bool screenClick = false;
    public KeyCode interactKey = KeyCode.Mouse1;
    public Interactable focus;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation so we can control it manually

        slider.onValueChanged.AddListener(delegate { SetCameraSpeed(slider.value); });
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
            HandleInput();
            Move();

            if (camIndex == 0)
            {
                Rotate();
            }
            else
            {
                RotateAround();
            }

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
            Ray ray;

            // Determine the origin of the ray based on screenClick
            if (screenClick)
            {
                // Raycast from the camera at the screen center
                ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            }
            else
            {
                // Raycast from the transform's position
                ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            }

            // Perform the raycast and store the result
            if (Physics.Raycast(ray, out RaycastHit hitInfo, reachDistance, ~playerLayer))
            {
                Debug.Log("Hit: " + hitInfo.collider.name);
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

                // Perform actions on hit (e.g., interact with the object)
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
            if (focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }

    public void SetCameraSpeed(float camRotSpe)
    {
        // Debug.Log("New camera speed:" + camRotSpe);
        rotationSpeed = camRotSpe;
    }



    bool HandleInput()
    {
        bool actionFlag = false;



        if (Input.GetKeyDown(hangKey) && !isGrounded)
        {
            TryHangOnWall();
            actionFlag = true;
        }

        if (canGrab && Input.GetKeyDown(ledgeGrabKey))
        {
            TryGrabLedge();
            actionFlag = true;
        }

        if (Input.GetKeyDown(cameraKey))
        {
            SwitchPerspective();
            actionFlag = true;
        }

        HandleMovementInput();
        return actionFlag;
    }

    void SwitchPerspective()
    {
        camIndex = (camIndex + 1) % camPositions.Length;
        SetCameraPosition(camIndex);
    }

    void SetCameraPosition(int index)
    {
        if (index < 0 || index >= camPositions.Length)
        {
            Debug.LogError("Index out of bounds!");
            return;
        }

        playerCamera.transform.localPosition = camPositions[index].position;
        playerCamera.transform.localRotation = camPositions[index].rotation;
    }

    void HandleMovementInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get the forward and right vectors of the camera
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

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
            if (horizontalSpeed > maxGroundedSpeed)
            {
                // Calculate the velocity vector with the same direction but capped magnitude
                Vector3 clampedVelocity = horizontalVelocity.normalized * maxGroundedSpeed;
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


    void Move()
    {
        // Additional movement logic (if any) can be added here
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationY += mouseX;
        rotationX = Mathf.Clamp(rotationX + mouseY, -90, 90);

        // Calculate the target rotation for the camera
        Quaternion targetCameraRotation = Quaternion.Euler(rotationX, 0, 0);
        playerCamera.transform.localRotation = Quaternion.Lerp(playerCamera.transform.localRotation, targetCameraRotation, Time.fixedDeltaTime * rotationSmoothness);

        // Calculate the target rotation for the player object (around the Y-axis)
        Quaternion playerTargetRotation = Quaternion.Euler(0, rotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRotation, Time.fixedDeltaTime * rotationSmoothness);
    }
    void RotateAround()
    {
        // Define the point to rotate around (usually the player's position)
        Vector3 rotationPoint = transform.position;

        // Calculate rotation based on mouse input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationY += mouseX;
        Quaternion playerTargetRotation = Quaternion.Euler(0, rotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRotation, Time.fixedDeltaTime * rotationSmoothness);

        // Rotate around the Y-axis for horizontal look
        // playerCamera.transform.RotateAround(rotationPoint, Vector3.up, mouseX);

        // Rotate around the X-axis for vertical look (pitch)
        Vector3 right = playerCamera.transform.right; // Right vector for pitch
        playerCamera.transform.RotateAround(rotationPoint, right, mouseY);

        // Clamp the vertical rotation (pitch) to avoid flipping
        // float currentXRotation = playerCamera.transform.eulerAngles.x;
        // if (currentXRotation > 180f)
        // {
        //     currentXRotation -= 360f; // Convert angle to -180 to 180 range
        // }
        // currentXRotation = Mathf.Clamp(currentXRotation, -85f, 85f);

        // Apply the clamped rotation
        // playerCamera.transform.eulerAngles = new Vector3(currentXRotation, playerCamera.transform.eulerAngles.y, 0);
        // Calculate the target rotation for the player object (around the Y-axis)

    }

    void TryGrabLedge()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, ledgeDetectionRange, ~playerLayer))
        {
            StartCoroutine(GrabLedge(hit.point, hit.normal));
        }
    }

    IEnumerator GrabLedge(Vector3 ledgePosition, Vector3 ledgeNormal)
    {
        canGrab = false;

        transform.position = ledgePosition;
        transform.rotation = Quaternion.LookRotation(-ledgeNormal);

        yield return new WaitForSeconds(grabCooldown);

        canGrab = true;
    }

    void TryHangOnWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, ~playerLayer))
        {
            HangOnWall(hit.transform);
        }
    }

    void HangOnWall(Transform wall)
    {
        isHanging = true;
        hangTimer = 0f;
        hangingWall = wall;
        rb.velocity = Vector3.zero;
    }

    void Hang()
    {
        hangTimer += Time.fixedDeltaTime;
        if (hangTimer >= wallHangTime)
        {
            isHanging = false;
        }

        if (isHanging && hangingWall != null)
        {
            transform.position += hangingWall.position - transform.position;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        // // Cast a ray downward from the center of the player's collider
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.3f, ~playerLayer);

    }

    void CheckForIdle()
    {
        // Check for low horizontal velocity
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 verticalVelocity = new Vector3(0, rb.velocity.y, 0);

        if (horizontalVelocity.magnitude < 1f && (verticalVelocity.magnitude == 0))
        {
            // Stop the player by setting the velocity to zero
        }
    }
}
