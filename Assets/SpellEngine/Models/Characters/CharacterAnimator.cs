using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    Animator animator;

    private Rigidbody rb;
    private CharacterActions ca;
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
        float speedPercent = rb.velocity.magnitude / ca.maxGroundedSpeed;
        animator.SetFloat("speedPercent", speedPercent, dampTime, Time.deltaTime);
    }
}
