using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    Animator animator;

    private Rigidbody rb;
    private CharacterActions ca;
    private float maxAnimSpeed = 10f;
    public float dampTime = .1f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        ca = GetComponent<CharacterActions>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Get user input (-1 to 1)
        float vertical = Input.GetAxis("Vertical");

        // Debug.Log("Running velocity and magnitude: " + rb.velocity.magnitude + " " + rb.velocity);
        float speedPercent = rb.velocity.magnitude / maxAnimSpeed;
        // Debug.Log("Max Ground Speeed and current speed percent: " + ca.maxGroundedSpeed + " " + speedPercent);
        // Debug.Log("horizontal: " + horizontal);
        // Debug.Log("Vertical: " + vertical);
        animator.SetFloat("speedPercent", speedPercent, dampTime, Time.deltaTime);
        animator.SetBool("grounded", ca.ReturnIsGrounded());
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

    }
}
